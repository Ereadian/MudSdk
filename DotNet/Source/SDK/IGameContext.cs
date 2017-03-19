//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameContext.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.Diagnostics;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement;

    public interface IGameContext : IDisposable
    {
        GameSettings Settings { get; }

        LocaleManager LocaleManager { get; }

        ColorManager ColorManager { get; }

        TypeManager TypeManager { get; }

        RoomManager RoomManager { get; }

        WorldManager WorldManager { get; }

        IContentStorage ContentStorage { get; }
        IProfileStorage ProfileStorage { get; }

        ILog Log { get; }
    }
}
