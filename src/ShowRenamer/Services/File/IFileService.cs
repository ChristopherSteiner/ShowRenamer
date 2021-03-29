using System.Collections;
using System.Collections.Generic;

namespace ShowRenamer.Services.File
{
    public interface IFileService
    {
        IEnumerable<string> GetFiles(string path, string pattern);

        string GetFileName(string path);

        string RemoveInvalidCharacters(string fileName);

        void RenameFile(string oldFilePath, string newFileName);
    }
}
