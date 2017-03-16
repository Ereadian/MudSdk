namespace Ereadian.MudSdk.Sdk.Runtime
{
    public static class ObjectFactory
    {
        public static IObjectFactory Instance
        {
            get
            {
                return Singleton<DefaultObjectFactory>.Instance;
            }
        }
    }
}
