//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Player.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.WorldManagement;

    /// <summary>
    /// Player instance
    /// </summary>
    public class Player : Creature
    {
        /// <summary>
        /// Command separator characters
        /// </summary>
        private static readonly char[] CommandSeparatorCharacters = new char[] { ';' };

        /// <summary>
        /// player input for processing
        /// </summary>
        private readonly Queue<string> inputs = new Queue<string>(100);

        /// <summary>
        /// message for player to render
        /// </summary>
        private readonly Queue<Message> outputs = new Queue<Message>(50);

        /// <summary>
        /// Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        /// <param name="game">game instance</param>
        /// <param name="client">player client</param>
        public Player(Game game, IClient client) : base(game.ActionableItemManager)
        {
            this.Client = client;
            this.CurrentGame = game;
            var world = game.Context.WorldManager.LoginWorld;
            world.Add(this);
            game.ActionableItemManager.Add(this);
        }

        /// <summary>
        /// Gets current game instance
        /// </summary>
        public Game CurrentGame { get; private set; }

        /// <summary>
        /// Gets or sets player locale id
        /// </summary>
        public int LocaleId { get; set; }

        /// <summary>
        /// Gets current client instance
        /// </summary>
        public IClient Client { get; private set; }

        /// <summary>
        /// Gets or sets current profile instance
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets current user world
        /// </summary>
        public IWorld World { get; set; }

        /// <summary>
        /// Gets or sets current player world runtime
        /// </summary>
        public IWorldRuntime WorldRuntime { get; set; }

        /// <summary>
        /// Add player input
        /// </summary>
        /// <param name="command">command player entered</param>
        public void AddInput(string command)
        {
            var items = command.Split(CommandSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
            lock (this.inputs)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    this.inputs.Enqueue(items[i]);
                }
            }
        }

        /// <summary>
        /// Add message for player to display
        /// </summary>
        /// <param name="message">message instance</param>
        public void AddOuput(Message message)
        {
            lock (this.outputs)
            {
                this.outputs.Enqueue(message);
            }
        }

        /// <summary>
        /// Get input from player input queue
        /// </summary>
        /// <returns>command from player</returns>
        public string GetInput()
        {
            string input = null;
            lock (this.inputs)
            {
                if (this.inputs.Count > 0)
                {
                    input = this.inputs.Dequeue();
                }
            }

            return input;
        }

        /// <summary>
        /// Process player work
        /// </summary>
        public override void Run()
        {
            lock (this.outputs)
            {
                var count = this.outputs.Count;
                if (count > 0)
                {
                    var messages = new Message[count];
                    for (var i = 0; i < count; i++)
                    {
                        messages[i] = this.outputs.Dequeue();
                    }

                    ThreadPool.QueueUserWorkItem(
                        ShowMessage, 
                        new KeyValuePair<Player, IReadOnlyList<Message>>(this, messages));
                }
            }

            this.World.Run(this);
        }

        /// <summary>
        /// Show message to player
        /// </summary>
        /// <param name="state">thread state</param>
        private static void ShowMessage(object state)
        {
            var data = (KeyValuePair<Player, IReadOnlyList<Message>>)state;
            var player = data.Key;
            var messages = data.Value;

            var client = player.Client;
            for (var i = 0; i < messages.Count; i++)
            {
                client.RenderMessage(messages[i], player.LocaleId);
            }
        }
    }
}
