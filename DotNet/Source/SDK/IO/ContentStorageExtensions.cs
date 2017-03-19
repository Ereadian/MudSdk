namespace Ereadian.MudSdk.Sdk.IO
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public static class ContentStorageExtensions
    {
        public static XmlElement LoadXml(this IContentStorage storage, string folder, string file)
        {
            var path = string.IsNullOrEmpty(folder) ? file : storage.CombinePath(folder, file);
            return storage.LoadXml(path);
        }

        public static XmlElement LoadXml(this IContentStorage storage, string path)
        {
            XmlElement element = null;
            if (storage.IsFileExist(path))
            {
                using (var stream = storage.OpenForRead(path))
                {
                    var document = new XmlDocument();
                    document.Load(stream);
                    element = document.DocumentElement;
                }
            }

            return element;
        }
    }
}
