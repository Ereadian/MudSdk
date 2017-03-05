//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ParameterContent.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Content for parameter id 
    /// </summary>
    public class ParameterContent : IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterContent" /> class.
        /// </summary>
        /// <param name="parameterId">parameter order id</param>
        public ParameterContent(int parameterId)
        {
            this.ParameterId = parameterId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterContent" /> class.
        /// </summary>
        /// <param name="content">raw content</param>
        /// <param name="start">start location</param>
        /// <param name="end">end location</param>
        public ParameterContent(string content, int start, int end)
        {
            var data = 0;
            start = ContentUtility.SkipBlank(content, start, end);
            for (var i = start; i < end; i++)
            {
                var c = content[i];
                if (!char.IsDigit(c))
                {
                    break;
                }

                data = (data * 10) + ((int)c - (int)'0');
            }

            this.ParameterId = data;
        }

        /// <summary>
        /// Gets parameter order id
        /// </summary>
        public int ParameterId { get; private set; }

        /// <summary>
        /// Gets content type
        /// </summary>
        public ContentType Type
        {
            get
            {
                return ContentType.Parameter;
            }
        }
    }
}
