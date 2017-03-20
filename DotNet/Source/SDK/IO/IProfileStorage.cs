namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public interface IProfileStorage
    {
        void Init(IGameContext context);
        bool TryRegisterProfile(string name, Guid id);
        Profile Load(string name, IGameContext context);
        void Save(Profile profile, IGameContext context);
    }
}
