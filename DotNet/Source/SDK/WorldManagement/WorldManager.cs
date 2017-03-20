//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="WorldManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.Diagnostics;

    /// <summary>
    /// World manager
    /// </summary>
    public class WorldManager
    {
        /// <summary>
        /// world collection
        /// </summary>
        private readonly IDictionary<string, IWorld> worlds;

        /// <summary>
        /// Login world name
        /// </summary>
        private readonly string loginWorldName;

        /// <summary>
        /// name of the world for new player
        /// </summary>
        private readonly string startWorldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="loginWorldName">name of the world for login</param>
        /// <param name="startWorldName">new of the world for new player</param>
        public WorldManager(string loginWorldName, string startWorldName)
        {
            this.worlds = new Dictionary<string, IWorld>(StringComparer.OrdinalIgnoreCase);
            this.loginWorldName = loginWorldName;
            this.startWorldName = startWorldName;
        }

        /// <summary>
        /// Gets the world for login
        /// </summary>
        public IWorld LoginWorld { get; private set; }

        /// <summary>
        /// Gets the world for new player start
        /// </summary>
        public IWorld StartWorld { get; private set; }

        /// <summary>
        /// Register a new world
        /// </summary>
        /// <param name="name">name of the world</param>
        /// <param name="world">world instance</param>
        /// <param name="log">error logger</param>
        public void RegisterWorld(string name, IWorld world, ILog log)
        {
            if (this.worlds.ContainsKey(name))
            {
                // TODO: write error. world exist
            }

            this.worlds.Add(name, world);

            if ((this.LoginWorld == null) 
                && name.Equals(this.loginWorldName, StringComparison.OrdinalIgnoreCase))
            {
                this.LoginWorld = world;
            }

            if ((this.StartWorld == null)
                && name.Equals(this.startWorldName, StringComparison.OrdinalIgnoreCase))
            {
                this.StartWorld = world;
            }
        }

        /// <summary>
        /// Get world by name
        /// </summary>
        /// <param name="name">name of the world</param>
        /// <param name="fallback">require fallback. if true and world was not found, fallback to start world</param>
        /// <returns>world instance</returns>
        public IWorld GetWorld(string name, bool fallback = true)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IWorld world;
                if (this.worlds.TryGetValue(name, out world))
                {
                    return world;
                }
            }
            
            return fallback ? this.StartWorld : null;
        }
    }
}
