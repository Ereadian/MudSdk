namespace Ereadian.MudSdk.Sdk.Runtime
{
    public interface IObjectFactory
    {
        bool TryGetInstance<T>(out T instance, string name = null);
    }
}
