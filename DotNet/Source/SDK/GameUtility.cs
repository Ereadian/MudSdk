//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameUtility.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// SDK utility
    /// </summary>
    public static class GameUtility
    {
        /// <summary>
        /// Load XML file
        /// </summary>
        /// <param name="xmlFilePath">XML file path</param>
        /// <param name="throwExceptionIfNotFound">
        /// flag of throw exception when file was not found. 
        /// true: throw exception
        /// false: return null instead of throwing exception
        /// </param>
        /// <returns>XML root element</returns>
        public static XmlElement LoadXml(string xmlFilePath, bool throwExceptionIfNotFound = false)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
            {
                if (throwExceptionIfNotFound)
                {
                    throw new ArgumentException("file path should not be null or empty");
                }

                return null;
            }

            if (!File.Exists(xmlFilePath))
            {
                if (throwExceptionIfNotFound)
                {
                    throw new FileNotFoundException("File does not exist. Path:" + xmlFilePath);
                }

                return null;
            }

            XmlElement rootElement;
            using (var stream = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read))
            {
                var document = new XmlDocument();
                document.Load(stream);
                rootElement = document.DocumentElement;
            }

            return rootElement;
        }

        public static string GetFilenameById(Guid id)
        {
            return id.ToString("N") + ".xml";
        }
    }
}
