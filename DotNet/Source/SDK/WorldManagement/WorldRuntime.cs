namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    public class WorldRuntime : IWorldRuntime
    {
        public WorldRuntime(IWorld world)
        {
            this.World = world;
        }

        public IWorld World { get; private set; }
    }
}
