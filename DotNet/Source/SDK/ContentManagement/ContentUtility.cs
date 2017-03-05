//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ContentUtility.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Content operation utility
    /// </summary>
    public static class ContentUtility
    {
        /// <summary>
        /// Content processors
        /// </summary>
        private static readonly IReadOnlyList<KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorIndex, IContent>>> ContentProcessors
            = new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorIndex, IContent>>[]
            {
                new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorIndex, IContent>>(
                    new KeyValuePair<string, string>("<%", "%>"),
                    (content, start, end, colorIndex) => new ColorContent(content, start, end, colorIndex)),
                new KeyValuePair<KeyValuePair<string, string>, Func<string, int, int, ColorIndex, IContent>>(
                    new KeyValuePair<string, string>("<#", "#>"),
                    (content, start, end, colorIndex) => new ParameterContent(content, start, end)),
            };

        /// <summary>
        /// Skip blank
        /// </summary>
        /// <param name="content">content to process</param>
        /// <param name="start">start location</param>
        /// <param name="end">end location</param>
        /// <returns>new location after skipped blank</returns>
        public static int SkipBlank(string content, int start, int end)
        {
            while ((start < end) && !char.IsLetterOrDigit(content[start]))
            {
                start++;
            }

            return start;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <param name="content">content to process</param>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <returns>token from content</returns>
        public static string GetToken(string content, int start, int end)
        {
            int current;
            for (current = start; (current < end) && char.IsLetterOrDigit(content[current]); current++)
            {
            }

            var length = current - start;
            return length == 0 ? string.Empty : content.Substring(start, length);
        }

        /// <summary>
        /// Get content list from raw text
        /// </summary>
        /// <param name="text">raw text</param>
        /// <param name="colorIndex">color index</param>
        /// <returns>content list</returns>
        public static IReadOnlyList<IContent> GetContent(string text, ColorIndex colorIndex)
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

            return (list != null) && (list.Count > 0) ? list.ToArray() : null;
        }

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
                for (var id = 0; position < ContentProcessors.Count; position++)
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
