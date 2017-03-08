namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    public class LoginWorld : World
    {
        private static readonly MD5 md5 = MD5.Create();
        private Message localeNames;

        public override void Init(string name, Game game)
        {
            base.Init(name, game);
            this.localeNames = CreateLocaleList(game);
        }

        public override void Run(Player player)
        {
            var runtime = this.GetRuntime<LoginWorldRuntime>(player.WorldRuntime);
            if (runtime == null)
            {
                return;
            }

            string userName = null;
            string password = null;
            string localeChoice = null;
            int localeId;

            switch (runtime.Status)
            {
                case LoginStatus.Enter:
                    player.AddOuput(ContentUtility.CreateMessage(GameTitle.Title));
                    player.AddOuput(this.localeNames);
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.SelectLocale));
                    runtime.Status = LoginStatus.SelectLocale;
                    break;
                case LoginStatus.SelectLocale:
                    localeChoice = player.GetInput();
                    if (string.IsNullOrWhiteSpace(localeChoice))
                    {
                        return;
                    }

                    if (!int.TryParse(localeChoice, out localeId) || (localeId < 1) || (localeId > player.CurrentGame.Locales.LocaleCount))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidLocaleId));
                        player.AddOuput(this.localeNames);
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.SelectLocale));
                        return;
                    }

                    runtime.LocaleId = --localeId;

                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterUserName, runtime.LocaleId));
                    runtime.Status = LoginStatus.EnterUserName;
                    break;
                case LoginStatus.EnterUserName:
                    userName = player.GetInput();
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        return;
                    }

                    userName = userName.Trim();
                    if (!VerifyInput(userName))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName, runtime.LocaleId));
                        return;
                    }

                    runtime.UserName = userName;
                    player.Profile = player.CurrentGame.PlayerManager.GetProfile(userName);
                    if (player.Profile == null)
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.NewUser, runtime.LocaleId));
                        runtime.Status = LoginStatus.CreateProfile;
                    }
                    else
                    {
                        player.Profile.LocaleId = runtime.LocaleId;
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterPassword, runtime.LocaleId));
                        runtime.Status = LoginStatus.VerifyPassword;
                    }

                    break;
                case LoginStatus.VerifyPassword:
                    password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    password = password.Trim();
                    if (!VerifyInput(password))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName, runtime.LocaleId));
                        return;
                    }

                    if (player.Profile.PasswordHash != GetHash(password))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidPassword, runtime.LocaleId));
                        return;
                    }

                    runtime.Password = password;
                    runtime.Status = LoginStatus.EnterWorld;
                    break;
                case LoginStatus.CreateProfile:
                    password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    password = password.Trim();
                    if (!VerifyInput(password))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName, runtime.LocaleId));
                        return;
                    }

                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.ConfirmPassword, runtime.LocaleId));
                    runtime.Password = password;
                    runtime.Status = LoginStatus.ConfirmPassword;
                    break;

                case LoginStatus.ConfirmPassword:
                    password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    if (password != runtime.Password)
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.PasswordNotMatch, runtime.LocaleId));
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.NewUser, runtime.LocaleId));
                        runtime.Status = LoginStatus.CreateProfile;
                        return;
                    }

                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.CreatingAccount, runtime.LocaleId));
                    player.Profile = new Profile(runtime.UserName, GetHash(runtime.Password));
                    player.Profile.World = player.CurrentGame.WorldManager.StartWorld;
                    runtime.Status = LoginStatus.EnterWorld;
                    break;
                case LoginStatus.EnterWorld:
                    player.Profile.LocaleId = runtime.LocaleId;
                    runtime.Status = LoginStatus.Transferring;
                    player.Profile.World.Add(player);
                    break;
            }
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new LoginWorldRuntime(this);
        }

        private void ShowLocaleList(Player player)
        {
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

        private static Message CreateLocaleList(Game game)
        {
            var locales = game.Locales;
            var list = new IContent[locales.LocaleCount];
            for (var i = 0; i < locales.LocaleCount; i++)
            {
                var culture = locales.GetCulture(i);
                var text = string.Format("{0}: {1}({2}){3}", i + 1, culture.NativeName, culture.EnglishName, Environment.NewLine);
                list[i] = new TextContent(text);
            }

            return new Message(list, null);
        }
    }
}
