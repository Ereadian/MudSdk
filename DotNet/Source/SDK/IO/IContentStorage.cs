namespace Ereadian.MudSdk.Sdk.IO
{
    using System.Collections.Generic;
    using System.IO;

    public interface IContentStorage
    {
        string CombinePath(string parentPath, string childPath, params string[] paths);
        Stream OpenForRead(string path);
        Stream OpenForWrite(string path);
        IReadOnlyList<string> GetFiles(string folder);
        bool IsFolderExist(string path);
        bool IsFileExist(string path);
    }
}
