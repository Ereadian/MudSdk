//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Game.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement.Login;
    using Ereadian.MudSdk.Sdk.WorldManagement.General;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public class Game
    {
        public ActionableObjectManager ActionableItemManager { get; private set; }

        public GameSettings Settings { get; private set; }

        public LocaleManager Locales { get; private set; }

        public ColorManager Colors { get; private set; }

        public RoomManager RoomManager { get; private set; }

        public PlayerManager PlayerManager { get; private set; }

        public Thread thread;

        public ManualResetEventSlim StopEvent { get; private set; }

        public WorldManager WorldManager { get; private set; }

        public virtual void Start(string gameFolder)
        {
            // load settings
            var settingsData = LoadData<GameSettingsData>(gameFolder, "game");
            this.Settings = new GameSettings(settingsData, Path.GetFullPath(gameFolder));

            // create color index
            this.Colors = new ColorManager();

            // create locale index
            this.Locales = new LocaleManager(this.Settings.DefaultLocale);

            // Load resource collection
            ResourceCollection.LoadResources(Path.Combine(gameFolder, "contents"), this.Locales, this.Colors);

            // load room manager
            this.RoomManager = new RoomManager(
                Path.Combine(gameFolder, "maps"), 
                this.Locales, 
                this.Colors);

            // Load World manager
            this.WorldManager = new WorldManager(settingsData.LoginWorldName, settingsData.StartWorldName);
            this.RegisterWorlds(settingsData);

            // create actionable manager
            this.ActionableItemManager = new ActionableObjectManager();

            // load player manager
            this.PlayerManager = new PlayerManager(gameFolder, this);

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
                this.ActionableItemManager.Run();
                stopwatch.Stop();
                timeout = this.Settings.HeartBeat - (int)stopwatch.ElapsedMilliseconds;
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

        public void RegisterWorld(string name, IWorld world)
        {
            this.WorldManager.RegisterWorld(name, world);
        }

        public IConnector Connect(IClient client)
        {
            return new Connector(this, client);
        }

        protected virtual void RegisterWorlds(GameSettingsData settings)
        {
            if (settings.Worlds != null)
            {
                for (var i = 0; i < settings.Worlds.Length; i++)
                {
                    var worldData = settings.Worlds[i];
                    IWorld world = null;
                    try
                    {
                        var type = Type.GetType(worldData.TypeName);
                        if (type != null)
                        {
                            world = Activator.CreateInstance(type) as IWorld;
                            world.Init(worldData.WorldName, this);
                        }
                    }
                    catch
                    {
                        world = null;
                    }

                    if (world != null)
                    {
                        this.RegisterWorld(worldData.WorldName, world);
                    }
                }
            }

            if (this.WorldManager.LoginWorld == null)
            {
                this.RegisterWorld(settings.LoginWorldName, new LoginWorld());
            }

            if (this.WorldManager.StartWorld == null)
            {
                this.RegisterWorld(settings.StartWorldName, new GeneralWorld());
            }
        }

        protected virtual void WriteConsole(string message)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }
        }

        private T LoadData<T>(string root, string file) where T: new()
        {
            var path = Path.GetFullPath(Path.Combine(root, file + ".xml"));
            if (!File.Exists(path))
            {
                this.WriteConsole("File does not exist: " + path);
                return default(T);
            }

            return Singleton<Serializer<T>>.Instance.Deserialize(path);
        }
    }
}
