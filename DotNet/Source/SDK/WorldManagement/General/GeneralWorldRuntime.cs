namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class GeneralWorldRuntime : WorldRuntime
    {
        public GeneralWorldRuntime(GeneralWorld world) : base(world)
        {
        }

        public WorldStatus Status { get; set; }

        public Room Room { get; set; }
    }
}
