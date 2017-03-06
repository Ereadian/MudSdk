//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="RoomsData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System.Xml.Serialization;

    /// <summary>
    /// Room data
    /// </summary>
    [XmlRoot("area")]
    public class AreaData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets room collection data
        /// </summary>
        [XmlArray("rooms")]
        [XmlArrayItem("room")]
        public RoomData[] Rooms { get; set; }
    }
}
