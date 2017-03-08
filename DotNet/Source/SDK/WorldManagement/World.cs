namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System.Threading;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public abstract class World : IWorld
    {
        private static volatile int nextWorldId = 0;

        public int Id { get; private set; }
        public string Name { get; private set; }

        public Room EntryRoom { get; protected set; }
        public Room RespawnRoom { get; protected set; }

        public World()
        {
            this.Id = Interlocked.Increment(ref nextWorldId);
        }

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

            player.WorldRuntime = this.CreateRuntime();
            player.World = this;
        }

        public virtual void Remove(Player player)
        {
            player.WorldRuntime = null;
        }

        public abstract void Run(Player player);

        protected abstract IWorldRuntime CreateRuntime();

        protected T GetRuntime<T>(IWorldRuntime runtime) where T : class, IWorldRuntime
        {
            var runtimeForTheWorld = runtime as T;
            if (runtimeForTheWorld != null)
            {
                var world = runtimeForTheWorld.World;
                if ((world != null) && (world.Id == this.Id))
                {
                    return runtimeForTheWorld;
                }
            }

            return null;
        }
    }
}
