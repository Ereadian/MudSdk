//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameSettingsData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System.Xml.Serialization;

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
    }
}
