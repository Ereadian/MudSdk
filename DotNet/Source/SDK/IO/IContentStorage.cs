namespace Ereadian.MudSdk.Sdk.IO
{
    using System.Collections.Generic;
    using System.IO;

    public interface IContentStorage
    {
        string CombinePath(string rootPath, string childPath);
        Stream OpenForRead(string path);
        Stream OpenForWrite(string path);
        IReadOnlyList<string> GetFiles(string folder);
        bool IsFileExist(string path);
    }
}
