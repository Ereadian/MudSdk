//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Game.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using System.IO;
    using System.Diagnostics;
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement.Login;

    public class Game
    {
        public ActionableObjectManager ActionableItemManager { get; private set; }

        public GameSettings Settings { get; private set; }

        public LocaleIndex Locals { get; private set; }

        public ColorIndex Colors { get; private set; }

        public IWorld LoginWorld { get; private set; }

        public Thread thread;

        public ManualResetEventSlim StopEvent { get; private set; }

        public virtual void Start(string gameFolder)
        {
            if (LoginWorld == null)
            {
                this.RegisterWorld(new LoginWorld());
            }

            // load settings
            this.Settings = new GameSettings(LoadData<GameSettingsData>(gameFolder, "game"));

            // create color index
            this.Colors = new ColorIndex();

            // create locale index
            this.Locals = new LocaleIndex(this.Settings.DefaultLocale);

            // Load resource collection
            ResourceCollection.LoadResources(Path.Combine(gameFolder, "contents"), this.Locals, this.Colors);

            // create actionable manager
            this.ActionableItemManager = new ActionableObjectManager();

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

        public void RegisterWorld(params IWorld[] worlds)
        {
            if (worlds != null)
            {
                for (var i = 0; i < worlds.Length; i++)
                {
                    var world = worlds[i];
                    if ((this.LoginWorld == null) && world.IsLogingWorld)
                    {
                        this.LoginWorld = world;
                    }
                }
            }
        }

        public IConnector Connect(IClient client)
        {
            return new Connector(this, client);
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
