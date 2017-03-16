namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class WorldManager
    {
        private readonly IDictionary<string, IWorld> worlds = new Dictionary<string, IWorld>(StringComparer.OrdinalIgnoreCase);

        private readonly string loginWorldName;
        private readonly string startWorldName;

        public WorldManager(string loginWorldName, string startWorldName)
        {
            this.loginWorldName = loginWorldName;
            this.startWorldName = startWorldName;
        }

        public IWorld LoginWorld { get; private set; }
        public IWorld StartWorld { get; private set; }

        public void RegisterWorld(string name, IWorld world)
        {
            if (!this.worlds.ContainsKey(name))
            {
                this.worlds.Add(name, world);
            }

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
