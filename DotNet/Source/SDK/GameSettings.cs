//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameSettings.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    /// <summary>
    /// Game settings data
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsData" /> class.
        /// </summary>
        public GameSettings(GameSettingsData settings)
        {
            this.DefaultLocale = settings.Locale;
            this.HeartBeat = settings.HeartBeat;
        }

        /// <summary>
        /// Gets or sets default locale name
        /// </summary>
        public string DefaultLocale { get; private set; }

        /// <summary>
        /// Gets or sets game heartbeat
        /// </summary>
        public int HeartBeat { get; private set; }
    }
}
