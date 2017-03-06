namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public interface IWorld
    {
        bool IsLogingWorld { get; }
        void Add(Player player);
        void Remove(Player player);
        void Run(Player player);
    }
}
