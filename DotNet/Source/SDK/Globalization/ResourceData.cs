//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ResourceData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Globalization
{
    using System.Xml.Serialization;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    /// <summary>
    /// Resource data
    /// </summary>
    [XmlRoot("resource")]
    public class ResourceData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// resource items
        /// </summary>
        [XmlElement("content")]
        public ContentData[] Resources { get; set; }
    }
}
