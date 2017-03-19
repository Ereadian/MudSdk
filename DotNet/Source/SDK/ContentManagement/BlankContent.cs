//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="BlankContent.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Blank content
    /// </summary>
    public class BlankContent : IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlankContent" /> class.
        /// </summary>
        /// <param name="text">whole content</param>
        /// <param name="start">blank text start position</param>
        /// <param name="end">blank text end position</param>
        public BlankContent(string text, int start, int end)
        {
            start = ContentUtility.SkipBlank(text, start, end);
            var data = ContentUtility.GetToken(text, start, end);
            this.Text = string.IsNullOrEmpty(data) ? string.Empty : data;

            start += data.Length;
            start = ContentUtility.SkipBlank(text, start, end);
            var countText = ContentUtility.GetToken(text, start, end);

            int count;
            this.Count = int.TryParse(countText, out count) ? count : 1;
        }

        /// <summary>
        /// Gets blank content
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets number of contents
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets content type
        /// </summary>
        public ContentType Type
        {
            get
            {
                return ContentType.Blank;
            }
        }
    }
}
