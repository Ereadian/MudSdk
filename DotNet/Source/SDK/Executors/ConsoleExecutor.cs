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

            var game = new Game();
            game.Start();

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
