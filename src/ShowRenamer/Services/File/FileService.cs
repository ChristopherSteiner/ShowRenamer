using System;
using System.Collections.Generic;
using System.IO;

namespace ShowRenamer.Services.File
{
    public class FileService : IFileService
    {
        public IEnumerable<string> GetFiles(string path, string pattern)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, pattern);
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

        public void RenameFile(string oldFilePath, string newFileName)
        {
            FileInfo fileInfo = new FileInfo(oldFilePath);
            fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newFileName));
        }
    }
}
