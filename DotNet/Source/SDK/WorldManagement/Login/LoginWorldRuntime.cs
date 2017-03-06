namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    public class LoginWorldRuntime : IWorldRuntime
    {
        public LoginWorldRuntime()
        {
            this.Status = LoginStatus.Enter;
        }

        public LoginStatus Status { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
