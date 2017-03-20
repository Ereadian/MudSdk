//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Content.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Content data
    /// </summary>
    public struct Content
    {
        /// <summary>
        /// Color start tag
        /// </summary>
        public const string ColorTagStart = "{%";

        /// <summary>
        /// Color end tag
        /// </summary>
        public const string ColorTagEnd = "%}";

        /// <summary>
        /// Parameter start tag
        /// </summary>
        public const string ParameterTagStart = "{#";

        /// <summary>
        /// Parameter end tag
        /// </summary>
        public const string ParameterTagEnd = "#}";

        /// <summary>
        /// Blank start tag
        /// </summary>
        public const string BlankTagStart = "{!";

        /// <summary>
        /// Blank end tag
        /// </summary>
        public const string BlankTagEnd = "!}";

        /// <summary>
        /// Content processors
        /// </summary>
        private static readonly IReadOnlyList<KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorManager, IContent>>> ContentProcessors
            = new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorManager, IContent>>[]
            {
                new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorManager, IContent>>(
                    new KeyValuePair<string, string>(ColorTagStart, ColorTagEnd),
                    (content, start, end, colorIndex) => new ColorContent(content, start, end, colorIndex)),
                new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorManager, IContent>>(
                    new KeyValuePair<string, string>(ParameterTagStart, ParameterTagEnd),
                    (content, start, end, colorIndex) => new ParameterContent(content, start, end)),
                new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorManager, IContent>>(
                    new KeyValuePair<string, string>(BlankTagStart, BlankTagEnd),
                    (content, start, end, colorIndex) => new BlankContent(content, start, end)),
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> struct.
        /// </summary>
        /// <param name="content">content data list</param>
        public Content(IReadOnlyList<IContent> content)
        {
            this.Data = content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> struct.
        /// </summary>
        /// <param name="text">simple text</param>
        public Content(string text)
        {
            this.Data = new IContent[]
            {
                new  TextContent(text)
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> struct.
        /// </summary>
        /// <param name="text">raw text</param>
        /// <param name="colorIndex">color index</param>
        /// <returns>content list</returns>
        public Content(string text, ColorManager colorIndex)
        {
            var list = new List<IContent>();
            if (text != null)
            {
                int start = 0;
                while (start < text.Length)
                {
                    int processorId;
                    var position = Match(text, start, out processorId);
                    if (position < 0)
                    {
                        list.Add(new TextContent(text, start, text.Length));
                        break;
                    }

                    if (position > start)
                    {
                        list.Add(new TextContent(text, start, position));
                    }

                    var processor = ContentProcessors[processorId];
                    start = position + processor.Key.Key.Length;
                    var endTag = processor.Key.Value;
                    position = text.IndexOf(endTag, start);
                    if (position < 0)
                    {
                        // TODO: log error
                        break;
                    }

                    list.Add(processor.Value(text, start, position, colorIndex));
                    start = position + endTag.Length;
                }
            }

            this.Data = (list != null) && (list.Count > 0) ? list.ToArray() : Empty<IContent>.Array;
        }

        /// <summary>
        /// Gets content data
        /// </summary>
        public IReadOnlyList<IContent> Data { get; private set; }

        /// <summary>
        /// Match processor
        /// </summary>
        /// <param name="content">content to process</param>
        /// <param name="start">start point</param>
        /// <param name="processorId">processor id</param>
        /// <returns>position which matches process start tag</returns>
        private static int Match(string content, int start, out int processorId)
        {
            for (var position = start; position < content.Length; position++)
            {
                for (var id = 0; id < ContentProcessors.Count; id++)
                {
                    var processor = ContentProcessors[id];
                    if (IsStartWith(content, processor.Key.Key, position))
                    {
                        processorId = id;
                        return position;
                    }
                }
            }

            processorId = -1;
            return -1;
        }

        /// <summary>
        /// is content start with prefix
        /// </summary>
        /// <param name="content">content to check</param>
        /// <param name="prefix">prefix to match</param>
        /// <param name="start">start location</param>
        /// <returns>true if matches</returns>
        private static bool IsStartWith(string content, string prefix, int start)
        {
            if (content.Length - start < prefix.Length)
            {
                return false;
            }

            for (var i = 0; i < prefix.Length; i++, start++)
            {
                if (content[start] != prefix[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
