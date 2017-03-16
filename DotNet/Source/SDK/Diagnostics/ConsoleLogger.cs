namespace Ereadian.MudSdk.Sdk.Diagnostics
{
    using System;

    public class ConsoleLogger : ILog
    {
        public void Write(LogLevel level, string message)
        {
            Console.WriteLine("[LOG:{0} - {1}]: {2}", level, DateTime.Now, message);
        }
    }
}
