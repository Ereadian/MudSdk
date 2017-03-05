//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ColorContent.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Color content
    /// </summary>
    public class ColorContent : IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorContent" /> class.
        /// </summary>
        /// <param name="foregroundColorId">foreground color id</param>
        /// <param name="backgroundColorId">background color id</param>
        public ColorContent(int foregroundColorId, int backgroundColorId)
        {
            this.ForegroundColorId = foregroundColorId;
            this.BackgroundColorId = backgroundColorId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorContent" /> class.
        /// </summary>
        /// <param name="text">raw text</param>
        /// <param name="start">start position</param>
        /// <param name="end">end position</param>
        /// <param name="colorIndex">color index</param>
        public ColorContent(string text, int start, int end, ColorIndex colorIndex)
        {
            start = ContentUtility.SkipBlank(text, start, end);
            var name = ContentUtility.GetToken(text, start, end);
            this.ForegroundColorId = colorIndex[name];

            start += name.Length;
            start = ContentUtility.SkipBlank(text, start, end);
            name = ContentUtility.GetToken(text, start, end);
            this.BackgroundColorId = colorIndex[name];
        }

        /// <summary>
        /// Gets foreground color id
        /// </summary>
        public int ForegroundColorId { get; private set; }

        /// <summary>
        /// Gets background color id
        /// </summary>
        public int BackgroundColorId { get; private set; }

        /// <summary>
        /// Gets content type
        /// </summary>
        public ContentType Type
        {
            get
            {
                return ContentType.Color;
            }
        }
    }
}
