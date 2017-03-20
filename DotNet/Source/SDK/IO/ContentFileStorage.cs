namespace Ereadian.MudSdk.Sdk.IO
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class ContentFileStorage : IContentStorage
    {
        public const char DirectorySeparatorChar = '/';

        private string rootFolder;

        public ContentFileStorage(string rootFolder = null, string childFolder = null)
        {
            if (string.IsNullOrEmpty(rootFolder))
            {
                rootFolder = ConfigurationManager.AppSettings["GameFolder"];
            }

            rootFolder = rootFolder.Trim();
            if (!string.IsNullOrWhiteSpace(childFolder))
            {
                childFolder = childFolder.Trim();
                if (DirectorySeparatorChar != Path.DirectorySeparatorChar)
                {
                    childFolder = childFolder.Replace(DirectorySeparatorChar, Path.DirectorySeparatorChar);
                }

                rootFolder = Combine(rootFolder, childFolder, null);
            }

            this.rootFolder = Path.GetFullPath(rootFolder);
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
        }

        public string CombinePath(string parentPath, string childPath, params string[] paths)
        {
            return Combine(parentPath, childPath, paths);
        }

        public Stream OpenForRead(string path)
        {
            var targetPath = FormalizePath(path);
            return new FileStream(Path.Combine(this.rootFolder, targetPath), FileMode.Open, FileAccess.Read);
        }

        public Stream OpenForWrite(string path)
        {
            var targetPath = Path.Combine(this.rootFolder, FormalizePath(path));
            var folder = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return new FileStream(targetPath, FileMode.Create, FileAccess.Write);
        }

        public IReadOnlyList<string> GetFiles(string folder)
        {
            var path = string.IsNullOrEmpty(folder) ? this.rootFolder : Path.Combine(this.rootFolder, FormalizePath(folder));
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

        public bool IsFolderExist(string path)
        {
            return Directory.Exists(Path.Combine(this.rootFolder, path));
        }

        public bool IsFileExist(string path)
        {
            return File.Exists(Path.Combine(this.rootFolder, path));
        }

        private static string Combine(string parentPath, string childPath, IReadOnlyList<string> pathList)
        {
            var build = new StringBuilder();
            build.Append(FormalizePath(parentPath));
            build.Append(Path.DirectorySeparatorChar);
            build.Append(FormalizePath(childPath));
            if (pathList != null)
            {
                for (var i = 0; i < pathList.Count; i++)
                {
                    build.Append(Path.DirectorySeparatorChar);
                    build.Append(FormalizePath(pathList[i]));
                }
            }

            return build.ToString();
        }

        private static string FormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            return path.Replace(DirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
