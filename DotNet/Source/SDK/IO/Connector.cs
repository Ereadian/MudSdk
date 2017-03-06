//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="IConnector.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    /// <summary>
    /// Connector interface
    /// </summary>
    public class Connector : IConnector
    {
        private Player player;

        public Connector(Game game, IClient client)
        {
            this.player = new Player(game, client);
        }

        /// <summary>
        /// Run user command
        /// </summary>
        /// <param name="command">command text</param>
        public void RunUserCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                this.player.AddInput(command);
            }
        }

        /// <summary>
        /// Disconnect from game
        /// </summary>
        public void Disconnect()
        {
        }
    }
}
