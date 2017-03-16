//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameContext.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using Ereadian.MudSdk.Sdk.Diagnostics;
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using Ereadian.MudSdk.Sdk.IO;

    public class GameContext
    {
        public GameContext(
            GameSettings settings, 
            LocaleManager localManager, 
            ColorManager colorManager,
            RoomManager roomManager,
            WorldManager worlderManager,
            IProfileStorage profileStorage,
            ILog log)
        {
            this.Settings = settings;
            this.LocaleManager = localManager;
            this.ColorManager = colorManager;
            this.RoomManager = roomManager;
            this.WorldManager = worlderManager;
            this.ProfileStorage = profileStorage;
            this.Log = log;
        }

        public GameSettings Settings { get; private set; }

        public LocaleManager LocaleManager { get; private set; }

        public ColorManager ColorManager { get; private set; }

        public RoomManager RoomManager { get; private set; }

        public WorldManager WorldManager { get; private set; }

        public IProfileStorage ProfileStorage { get; private set; }

        public ILog Log { get; private set; }
    }
}
