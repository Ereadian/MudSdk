//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="XmlExtensions.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Xml;

    public static class XmlExtensions
    {
        public static string GetChildElementText(this XmlElement element, string childElementPath)
        {
            string text = null;
            if (element != null)
            {
                var childElement = element.SelectSingleNode(childElementPath) as XmlElement;
                if (childElement != null)
                {
                    text = childElement.InnerText;
                }
            }

            return text;
        }

        public static T GetChildElementValue<T>(this XmlElement element, string childElementPath)
        {
            return element.GetChildElementValue<T>(childElementPath, default(T));
        }

        public static T GetChildElementValue<T>(this XmlElement element, string childElementPath, T defaultValue)
        {
            var rawData = element.GetChildElementText(childElementPath);
            if (!string.IsNullOrEmpty(rawData))
            {
                return (T)Convert.ChangeType(rawData, typeof(T));
            }

            return defaultValue;
        }

        public static XmlElement AddTextElement(this XmlElement element, string name, string text, bool useCData = false)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var document = element.OwnerDocument;
            var childElement = document.CreateElement(name);

            XmlNode dataNode;
            if (useCData)
            {
                dataNode = document.CreateCDataSection(text);
            }
            else
            {
                dataNode = document.CreateTextNode(text);
            }

            childElement.AppendChild(dataNode);
            element.AppendChild(childElement);
            return childElement;
        }
    }
}
