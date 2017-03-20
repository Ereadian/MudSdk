//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Resource.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    /// <summary>
    /// Resource data
    /// </summary>
    public struct Resource
    {
        /// <summary>
        /// name of content element
        /// </summary>
        public const string ContentElementName = "content";

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> struct.
        /// </summary>
        /// <param name="text">raw text</param>
        public Resource(string text) : this(new Text[] { new Text(LocaleManager.DefaultLocaleId, new Content(text)) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> struct.
        /// </summary>
        /// <param name="resourceElement">resource element</param>
        /// <param name="localeManager">locale manager</param>
        /// <param name="colorManager">color manager</param>
        public Resource(XmlElement resourceElement, LocaleManager localeManager, ColorManager colorManager)
            : this(LoadTextFromXml(resourceElement, localeManager, colorManager))
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="data">resource data</param>
        public Resource(IReadOnlyList<Text> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets resource data
        /// </summary>
        public IReadOnlyList<Text> Data { get; private set; }

        /// <summary>
        /// Gets content from resource by locale id
        /// </summary>
        /// <param name="localeId">locale id</param>
        /// <returns>content of the locale</returns>
        public Content this[int localeId]
        {
            get
            {
                if ((this.Data == null) || (this.Data.Count < 1))
                {
                    return default(Content);
                }

                for (var i = 0; i < this.Data.Count; i++)
                {
                    var content = this.Data[i];
                    if (content.LocaleId == localeId)
                    {
                        return content.Content;
                    }
                }

                if (localeId == LocaleManager.DefaultLocaleId)
                {
                    return this.Data[0].Content;
                }

                return this[LocaleManager.DefaultLocaleId];
            }
        }

        /// <summary>
        /// Load resource from XML
        /// </summary>
        /// <param name="resourceElement">resource XML element</param>
        /// <param name="localeManager">locale manager</param>
        /// <param name="colorManager">color manager</param>
        /// <returns>resource instance</returns>
        private static IReadOnlyList<Text> LoadTextFromXml(
            XmlElement resourceElement, 
            LocaleManager localeManager, 
            ColorManager colorManager)
        {
            List<Text> textList = null;
            foreach (XmlElement contentElement in resourceElement.SelectNodes(ContentElementName))
            {
                var text = new Text(contentElement, localeManager, colorManager);
                if (textList == null)
                {
                    textList = new List<Text>();
                }

                textList.Add(text);
            }

            return (textList != null) || (textList.Count < 1) ? Empty<Text>.Array : textList.ToArray(); 
        }
    }
}