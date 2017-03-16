namespace Ereadian.MudSdk.Sdk.Diagnostics
{
    public interface ILog
    {
        void Write(LogLevel level, string message);
    }
}
