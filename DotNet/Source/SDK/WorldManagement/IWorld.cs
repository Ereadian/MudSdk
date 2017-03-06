namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public interface IWorld
    {
        string Name { get; }

        void Init(string name, Game game);

        void Add(Player player);
        void Remove(Player player);
        void Run(Player player);

        Room EntryRoom { get; }
        Room RespawnRoom { get; }
    }
}
