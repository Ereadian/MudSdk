//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Game.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using System.IO;
    using Globalization;

    public class Game
    {
        private GameSettings settings;
        private LocaleIndex localIndex;
        private ColorIndex colors;
        private IReadOnlyList<Text> gameTitles;

        public virtual void Start(string gameFolder)
        {
            // load settings
            this.settings = new GameSettings(LoadData<GameSettingsData>(gameFolder, "game"));

            // create color index
            this.colors = new ColorIndex();

            // create locale index
            this.localIndex = new LocaleIndex(this.settings.DefaultLocale);

            // load title
            var titleData = LoadData<ResourceData>(gameFolder, "title");
            var title = new Text[titleData.Resources.Length];
            this.gameTitles = title;
            for(var i=0;i<titleData.Resources.Length;i++)
            {
                var resource = titleData.Resources[i];
                var localeId = this.localIndex.GetLocaleId(resource.Locale);
                var content = ContentUtility.FormalizeContent(resource.Data, this.colors);
                title[i] = new Text(localeId, content);
            }
        }

        public virtual void Stop()
        {
        }

        public IConnector Connect(IClient connector)
        {
            connector.RenderMessage(ContentUtility.GetContent(gameTitles, LocaleIndex.DefaultLocaleId), this.colors, null);
            return new Connector();
        }

        public void Disconnect(IConnector connector)
        {
        }

        protected virtual void WriteConsole(string message)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }
        }

        private T LoadData<T>(string root, string file) where T: new()
        {
            var path = Path.GetFullPath(Path.Combine(root, file + ".xml"));
            if (!File.Exists(path))
            {
                this.WriteConsole("File does not exist: " + path);
                return default(T);
            }

            return Singleton<Serializer<T>>.Instance.Deserialize(path);
        }
    }
}
