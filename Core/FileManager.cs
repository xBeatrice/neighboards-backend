using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication3.Core.Interfaces;

namespace WebApplication3.Core
{
    public class FileManager : IFileManager
    {
        private string BasePath = @"C:\Users\Andrei\Downloads\Users";
        private string FileExtension = ".txt";

      

        public void WriteContent(string fileName, string content)
        {
            string filePath = GetPath(fileName);
            File.WriteAllText(filePath, content);
        }

        public string GetContent(string fileName)
        {
            string filePath = GetPath(fileName);
            string fileContent = File.ReadAllText(filePath);
            return fileContent;
        }

        private string GetPath(string fileName)
        {
            if(!fileName.EndsWith(FileExtension))
            {
                fileName = fileName + FileExtension;
            }

            string filePath = Path.Combine(BasePath, fileName);
            return filePath;
        }
    }
}