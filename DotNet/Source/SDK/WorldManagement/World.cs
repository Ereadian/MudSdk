namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public abstract class World : IWorld
    {
        public string Name { get; private set; }

        public Room EntryRoom { get; protected set; }
        public Room RespawnRoom { get; protected set; }

        public virtual void Init(string name, Game game)
        {
            this.Name = name;
        }

        public virtual void Add(Player player)
        {
            if (player.World != null)
            {
                if (object.ReferenceEquals(this, player.World))
                {
                    return;
                }

                player.World.Remove(player);
            }

            player.World = this;
            player.WorldRuntime = this.CreateRuntime();
        }

        public virtual void Remove(Player player)
        {
            player.WorldRuntime = null;
        }

        public abstract void Run(Player player);

        protected abstract IWorldRuntime CreateRuntime();
    }
}
