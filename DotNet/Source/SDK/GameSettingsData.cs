//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameSettingsData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    /// <summary>
    /// Game settings data
    /// </summary>
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
        public string Locale { get; set; }
    }
}
