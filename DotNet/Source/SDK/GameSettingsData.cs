//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameSettingsData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Ereadian.MudSdk.Sdk.WorldManagement;

    /// <summary>
    /// Game settings data
    /// </summary>
    [XmlRoot("settings")]
    public class GameSettingsData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsData" /> class.
        /// </summary>
        public GameSettingsData()
        {
            this.Locale = "en-us";
        }

        /// <summary>
        /// Gets or sets default locale name
        /// </summary>
        [XmlElement("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets game heartbeat
        /// </summary>
        [XmlElement("tick")]
        public int HeartBeat { get; set; }

        [XmlElement("login")]
        public string LoginWorldName { get; set; }

        [XmlElement("start")]
        public string StartWorldName { get; set; }

        /// <summary>
        /// Gets or sets worlds
        /// </summary>
        [XmlArray("worlds")]
        [XmlArrayItem("world")]
        public WorldTypeData[] Worlds { get; set; }
    }
}
