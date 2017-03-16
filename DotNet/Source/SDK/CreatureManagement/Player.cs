//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Player.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class Player : Creature
    {
        /// <summary>
        /// Command separator characters
        /// </summary>
        private static readonly char[] CommandSeparatorCharacters = new char[] { ';' };

        private readonly Queue<string> Inputs = new Queue<string>(100);
        private readonly Queue<Message> Outputs = new Queue<Message>(50);

        public Player(Game game, IClient client) : base(game.ActionableItemManager)
        {
            this.Client = client;
            this.CurrentGame = game;
            var world = game.Context.WorldManager.LoginWorld;
            world.Add(this);
            game.ActionableItemManager.Add(this);
        }

        public Game CurrentGame { get; private set; }
        public IClient Client { get; private set; }
        public Profile Profile { get; set; }

        public IWorld World { get; set; }
        public IWorldRuntime WorldRuntime { get; set; }

        public void AddInput(string command)
        {
            var items = command.Split(CommandSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
            lock (this.Inputs)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    this.Inputs.Enqueue(items[i]);
                }
            }
        }

        public void AddOuput(Message message)
        {
            lock (this.Outputs)
            {
                this.Outputs.Enqueue(message);
            }
        }

        public string GetInput()
        {
            string input = null;
            lock (this.Inputs)
            {
                if (this.Inputs.Count > 0)
                {
                    input = this.Inputs.Dequeue();
                }
            }

            return input;
        }

        public override void Run()
        {
            lock (this.Outputs)
            {
                var count = this.Outputs.Count;
                if (count > 0)
                {
                    var messages = new Message[count];
                    for (var i = 0; i < count; i++)
                    {
                        messages[i] = this.Outputs.Dequeue();
                    }

                    ThreadPool.QueueUserWorkItem(
                        ShowMessage, 
                        new KeyValuePair<Player, IReadOnlyList<Message>>(this, messages));
                }
            }

            this.World.Run(this);
        }

        private static void ShowMessage(object state)
        {
            var data = (KeyValuePair<Player, IReadOnlyList<Message>>)state;
            var player = data.Key;
            var messages = data.Value;

            var client = player.Client;
            for (var i = 0; i < messages.Count; i++)
            {
                client.RenderMessage(messages[i]);
            }
        }
    }
}
