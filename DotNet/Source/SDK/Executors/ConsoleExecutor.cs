//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ConsoleExecutor.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Executors
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using Ereadian.MudSdk.Sdk.IO;

    public class ConsoleExecutor
    {
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;

        public virtual void Run(string[] args)
        {
            Console.InputEncoding = DefaultEncoding;
            Console.OutputEncoding = DefaultEncoding;

            string gamefolder;
            if (args.Length < 1)
            {
                Console.WriteLine("please specify game data folder");
                return;
            }

            gamefolder = Path.GetFullPath(args[0]);
            if (!Directory.Exists(gamefolder))
            {
                Console.WriteLine("Game folder does not exist: {0}", gamefolder);
                return;
            }

            var game = new Game();
            var contentStorage = new ContentFileStorage();
            var profileStorage = new ProfileFileStorage(new ContentFileStorage(null, "users/profile"));
            game.Start(contentStorage, profileStorage, null);

            var quit = false;
            var client = new ConsoleClient(game, () => quit = true);
            var connector = game.Connect(client);

            while (!quit)
            {
                var command = Console.ReadLine();
                if (!quit)
                {
                    connector.RunUserCommand(command);
                }
            }
        }
    }
}
