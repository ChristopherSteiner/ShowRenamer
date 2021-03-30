using ShowRenamer.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShowRenamer.Services.File
{
    public class FileService : IFileService
    {
        public IEnumerable<string> GetFiles(string path, string pattern, bool includeSubfolders)
        {
            if (Directory.Exists(path))
            {
                if (includeSubfolders)
                {
                    return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
                }
            }
            else
            {
                throw new ArgumentException($"Provided path '{path}' was not valid");
            }
        }

        public string GetFileName(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return Path.GetFileName(path);
            }
            else
            {
                throw new ArgumentException($"Provided path '{path}' was not valid");
            }
        }

        public string RemoveInvalidCharacters(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c.ToString(), "");
            }

            return fileName;
        }

        public void RenameFile(FileModel file, string newFileName, bool copyToMainfolder)
        {
            FileInfo fileInfo = new FileInfo(file.Path);
            if (copyToMainfolder)
            {
                fileInfo.MoveTo(Path.Combine(file.RootPath, newFileName));
            }
            else
            {
                fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newFileName));
            }
        }
    }
}
