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
    public class ContentData
    {
        /// <summary>
        /// Gets or sets localize name
        /// </summary>
        [XmlAttribute("locale")]
        public string Locale { get; set; }

        [XmlElement("data")]
        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Data { get; set; }
    }
}
