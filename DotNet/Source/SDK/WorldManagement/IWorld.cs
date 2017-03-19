namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public interface IWorld
    {
        int Id { get; }

        string Name { get; }

        IGameContext GameConext { get; }

        void Init(string name, IGameContext context);

        void Add(Player player);
        void Remove(Player player);
        void Run(Player player);

        IRoom EntryRoom { get; }
        IRoom RespawnRoom { get; }
    }
}
