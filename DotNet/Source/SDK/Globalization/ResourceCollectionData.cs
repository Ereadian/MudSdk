//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ResourceCollectionData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Globalization
{
    using System.Xml.Serialization;

    /// <summary>
    /// Resource data
    /// </summary>
    [XmlRoot("collection")]
    public class ResourceCollectionData
    {
        /// <summary>
        /// Gets collection name
        /// </summary>
        [XmlAttribute("name")]
        public string CollectionName { get; set; }

        /// <summary>
        /// Resource list
        /// </summary>
        [XmlArray("resources")]
        [XmlArrayItem("resource")]
        public ResourceData[] Resources { get; set; }
    }
}
