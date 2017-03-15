//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ereadian.MudSdk.Sdk.Globalization;
    using System.Xml.Serialization;

    /// <summary>
    /// Room data
    /// </summary>
    [XmlRoot("room")]
    public class RoomData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets room description in different languages
        /// </summary>
        [XmlArray("title")]
        [XmlArrayItem("content")]
        public ContentData[] Title { get; set; }

        /// <summary>
        /// Gets or sets room description in different languages
        /// </summary>
        [XmlArray("description")]
        [XmlArrayItem("content")]
        public ContentData[] Description { get; set; }

        /// <summary>
        /// Gets or sets room description in different languages
        /// </summary>
        [XmlArray("outlets")]
        [XmlArrayItem("outlet")]
        public Outlet[] Outlets { get; set; }

        [XmlRoot("outlet")]
        public class Outlet
        {
            [XmlAttribute("command")]
            public string Command { get; set; }

            [XmlAttribute("room")]
            public string RoomName { get; set; }
        }
    }
}
