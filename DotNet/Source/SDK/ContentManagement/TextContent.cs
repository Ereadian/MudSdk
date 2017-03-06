//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="TextContent.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Test content
    /// </summary>
    public class TextContent : IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent" /> class.
        /// </summary>
        /// <param name="text"></param>
        public TextContent(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextContent" /> class.
        /// </summary>
        /// <param name="content">raw content</param>
        /// <param name="start">start position</param>
        /// <param name="end">end position</param>
        public TextContent(string content, int start, int end)
        {
            var length = end - start;
            if (length < 1)
            {
                this.Text = string.Empty;
            }
            else if (length == content.Length)
            {
                this.Text = content;
            }
            else
            {
                this.Text = content.Substring(start, length);
            }
        }

        /// <summary>
        /// Gets text
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets content type
        /// </summary>
        public ContentType Type
        {
            get
            {
                return ContentType.Text;
            }
        }
    }
}
