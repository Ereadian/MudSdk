//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Text.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System.Xml;

    /// <summary>
    /// Text structure
    /// </summary>
    public struct Text
    {
        /// <summary>
        /// Locale attribute name
        /// </summary>
        public const string LocaleAttributeName = "locale";

        /// <summary>
        /// Initializes a new instance of the <see cref="Text" /> structure.
        /// </summary>
        /// <param name="localeId">locale id</param>
        /// <param name="content">formalized contents</param>
        public Text(int localeId, Content content)
        {
            this.LocaleId = localeId;
            this.Content = content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text" /> structure.
        /// </summary>
        /// <param name="contentElement">content XML element</param>
        /// <param name="localeManager">locale manager</param>
        /// <param name="colorManager">color manager</param>
        /// <example>
        /// <![CDATA[
        /// XML:
        /// <content locale="en-us">this is my content</content>
        /// ]]>
        /// </example>
        public Text(XmlElement contentElement, LocaleManager localeManager, ColorManager colorManager)
            : this(
                  localeManager.GetLocaleId(contentElement.GetAttribute(LocaleAttributeName)),
                  new Content(contentElement.InnerText, colorManager))
        {
        }

        /// <summary>
        /// Gets locale id of current content
        /// </summary>
        public int LocaleId { get; private set; }

        /// <summary>
        /// Gets formalized contents (color, text as so on)
        /// </summary>
        public Content Content { get; private set; }
    }
}
