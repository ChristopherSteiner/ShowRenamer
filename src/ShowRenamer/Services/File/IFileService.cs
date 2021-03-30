using ShowRenamer.Models;
using System.Collections;
using System.Collections.Generic;

namespace ShowRenamer.Services.File
{
    public interface IFileService
    {
        IEnumerable<string> GetFiles(string path, string pattern, bool includeSubfolders);

        string GetFileName(string path);

        string RemoveInvalidCharacters(string fileName);

        void RenameFile(FileModel file, string newFileName, bool copyToMainfolder);
    }
}
