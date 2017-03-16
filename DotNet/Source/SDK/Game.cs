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
    using Ereadian.MudSdk.Sdk.Diagnostics;

    public class Game
    {
        public GameContext Context { get; private set; }

        public ActionableObjectManager ActionableItemManager { get; private set; }

        public Thread thread;

        public ManualResetEventSlim StopEvent { get; private set; }

        public virtual void Start(string gameFolder, IProfileStorage profileStorage)
        {
            // load settings
            var settingsData = LoadData<GameSettingsData>(gameFolder, "game");
            var settings = new GameSettings(settingsData, Path.GetFullPath(gameFolder));

            // create color index
            var colors = new ColorManager();

            // create locale index
            var locales = new LocaleManager(settings.DefaultLocale);

            // Load resource collection
            ResourceCollection.LoadResources(Path.Combine(gameFolder, "contents"), locales, colors);

            // load room manager
            var roomManager = new RoomManager(Path.Combine(gameFolder, "maps"), locales, colors);

            // Load World manager
            var worldManager = new WorldManager(settingsData.LoginWorldName, settingsData.StartWorldName);
            this.RegisterWorlds(worldManager, settingsData);

            // create actionable manager
            this.ActionableItemManager = new ActionableObjectManager();

            this.Context = new GameContext(
                settings,
                locales,
                colors,
                roomManager,
                worldManager,
                profileStorage,
                new ConsoleLogger());

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
            return new Connector(this, client);
        }

        protected virtual void RegisterWorlds(WorldManager worldManager, GameSettingsData settings)
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
                       worldManager.RegisterWorld(worldData.WorldName, world);
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
