namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public interface IProfileStorage
    {
        bool TryRegisterProfile(string name, Guid id);
        Profile Load(string name);
        void Save(Profile profile);
    }
}
