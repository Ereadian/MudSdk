//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="DescriptionData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Globalization
{
    using System.Xml.Serialization;

    /// <summary>
    /// Description data
    /// </summary>
    [XmlRoot("content")]
    public class ContentData
    {
        /// <summary>
        /// Gets or sets localize name
        /// </summary>
        [XmlAttribute("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }
    }
}
