namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class LoginWorld : World
    {
        private static readonly MD5 md5 = MD5.Create();


        public override void Run(Player player)
        {
            var runtime = player.WorldRuntime as LoginWorldRuntime;
            switch (runtime.Status)
            {
                case LoginStatus.Enter:
                    player.AddOuput(ContentUtility.CreateMessage(GameTitle.Title));
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterUserName));
                    runtime.Status = LoginStatus.UserName;
                    break;
                case LoginStatus.UserName:
                    var name = player.GetInput();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return;
                    }

                    name = name.Trim();
                    if (!VerifyInput(name))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName));
                        return;
                    }

                    runtime.UserName = name;
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterPassword));
                    runtime.Status = LoginStatus.Password;
                    break;
                case LoginStatus.Password:
                    var password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    password = password.Trim();
                    if (!VerifyInput(password))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName));
                        return;
                    }

                    runtime.PasswordHash = GetHash(password);
                    runtime.Status = LoginStatus.Password;
                    break;
            }
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new LoginWorldRuntime();
        }

        private static bool VerifyInput(string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (!char.IsLetterOrDigit(c) && !(c == '.'))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetHash(string input)
        {

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerifyHash(string input, string hash)
        {
            var hashOfInput = GetHash(input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
