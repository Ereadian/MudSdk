//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Game.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.Diagnostics;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement.General;
    using Ereadian.MudSdk.Sdk.WorldManagement.Login;

    public class Game
    {
        public IGameContext Context { get; private set; }

        public Thread thread;

        public ManualResetEventSlim StopEvent { get; private set; }

        public virtual void Start(
            IContentStorage contentStorage = null,
            IProfileStorage profileStorage = null,
            ILog log = null)
        {
            if (contentStorage == null)
            {
                contentStorage = new ContentFileStorage();
            }

            if (profileStorage == null)
            {
                profileStorage = new ProfileFileStorage(contentStorage);
            }

            var context = new GameContext();
            this.Context = context;

            context.Log = log ?? new ConsoleLogger();
            context.ContentStorage = contentStorage;
            context.ProfileStorage = profileStorage;
            context.Settings = new GameSettings(contentStorage);
            context.ColorManager = new ColorManager();
            context.LocaleManager = new LocaleManager(this.Context.Settings.DefaultLocale);
            context.TypeManager = new TypeManager(contentStorage);

            // Load resource collection
            ResourceCollection.LoadResources(context);

            context.RoomManager = new RoomManager(context);
            var worldManager = new WorldManager(context.Settings.LoginWorldName, context.Settings.StartWorldName);
            this.RegisterWorlds(worldManager, context);
            context.WorldManager = worldManager;
            context.ActionableItemManager = new ActionableObjectManager();

            this.StopEvent = new ManualResetEventSlim(false);
            this.thread = new Thread(RunGame);
            this.thread.Start(this);
        }

        public virtual void Run()
        {
            var stopwatch = new Stopwatch();
            int timeout = 0;
            stopwatch.Start();
            while (!this.StopEvent.Wait(timeout))
            {
                this.Context.ActionableItemManager.Run();
                stopwatch.Stop();
                timeout = this.Context.Settings.HeartBeat - (int)stopwatch.ElapsedMilliseconds;
                if (timeout < 0)
                {
                    timeout = 0;
                }
            }
        }

        private static void RunGame(object state)
        {
            var game = state as Game;
            game.Run();
        }

        public virtual void Stop()
        {
        }

        public IConnector Connect(IClient client)
        {
            return new Connector(this.Context, client);
        }

        protected virtual void RegisterWorlds(WorldManager worldManager, IGameContext context)
        {
            GameSettings settings = context.Settings;
            if (settings.WorldTypes != null)
            {
                foreach (var pair in settings.WorldTypes)
                {
                    var worldName = pair.Key;
                    var worldType = pair.Value;
                    IWorld world = null;
                    try
                    {
                        world = Activator.CreateInstance(worldType) as IWorld;
                        world.Init(worldName, context);
                    }
                    catch
                    {
                        world = null;
                    }

                    if (world != null)
                    {
                        worldManager.RegisterWorld(worldName, world);
                    }
                }
            }

            if (worldManager.LoginWorld == null)
            {
                worldManager.RegisterWorld(settings.LoginWorldName, new LoginWorld());
            }

            if (worldManager.StartWorld == null)
            {
                worldManager.RegisterWorld(settings.StartWorldName, new GeneralWorld());
            }
        }

        protected virtual void WriteConsole(string message)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }
        }

        private class GameContext : IGameContext
        {
            public GameSettings Settings { get; internal set; }

            public LocaleManager LocaleManager { get; internal set; }

            public ColorManager ColorManager { get; internal set; }

            public ActionableObjectManager ActionableItemManager { get; internal set; }

            public TypeManager TypeManager { get; internal set; }

            public RoomManager RoomManager { get; internal set; }

            public WorldManager WorldManager { get; internal set; }

            public IContentStorage ContentStorage { get; internal set; }
            public IProfileStorage ProfileStorage { get; internal set; }

            public ILog Log { get; internal set; }

            #region IDisposable Support
            protected virtual void DisposeItems()
            {
                this.ColorManager.Dispose();
            }

            public void Dispose()
            {
                lock (this)
                {
                    if (this.ColorManager != null)
                    {
                        this.DisposeItems();
                    }
                }
            }
            #endregion IDisposable Support
        }
    }
}
