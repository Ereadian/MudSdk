namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Globalization;
    using System.Linq;

    public class ContentFileStorage : IContentStorage
    {
        private string rootFolder;

        public ContentFileStorage(string rootFolder)
        {
            if (string.IsNullOrEmpty(rootFolder))
            {
                throw new ArgumentException("root folder should not be null or empty");
            }

            rootFolder = rootFolder.Trim();
            this.rootFolder = rootFolder;
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
        }

        public string CombinePath(string rootPath, string childPath)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", rootPath, Path.DirectorySeparatorChar, childPath);
        }

        public Stream OpenForRead(string path)
        {
            return new FileStream(Path.Combine(this.rootFolder, path), FileMode.Open, FileAccess.Read);
        }

        public Stream OpenForWrite(string path)
        {
            return new FileStream(Path.Combine(this.rootFolder, path), FileMode.Create, FileAccess.Write);
        }

        public IReadOnlyList<string> GetFiles(string folder)
        {
            var path = string.IsNullOrEmpty(folder) ? this.rootFolder : Path.Combine(this.rootFolder, folder);
            var folderInformation = new DirectoryInfo(path);
            string[] files = null;

            if (folderInformation.Exists)
            {
                var fileInformationList = folderInformation.GetFiles();
                if (fileInformationList != null)
                {
                    files = new string[fileInformationList.Length];
                    for (var i = 0; i < fileInformationList.Length; i++)
                    {
                        files[i] = fileInformationList[i].Name;
                    }
                }
            }

            return files;
        }

        public bool IsFileExist(string path)
        {
            return File.Exists(Path.Combine(this.rootFolder, path));
        }
    }
}
