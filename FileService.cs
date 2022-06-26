using SnippetBatchProcessor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetBatchProcessor
{
    public class FileService : IFileService
    {
        public FileService() {}

        public void UploadFile()
        {
            //subDirectory = subDirectory ?? string.Empty;
            //var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);

            //Directory.CreateDirectory(target);

            //files.ForEach(async file =>
            //{
            //    if (file.Length <= 0) return;
            //    var filePath = Path.Combine(target, file.FileName);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(stream);
            //    }
            //});
        }

        public async Task<string> WriteTextFile(string destinationDirectory, string fileName, string textContents = "")
        {

            try
            {
                if (!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                if (string.IsNullOrEmpty(textContents))
                    textContents = string.Empty;

                var fullFileName = destinationDirectory + "\\" + fileName;
 
                await File.WriteAllTextAsync(fullFileName, textContents);

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string WriteMemoryStreamFile(MemoryStream fileMemoryStream, string fileName, string destinationDirectory)
        {
            try
            {
                if (!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                using (FileStream file = new FileStream(destinationDirectory + @"\" + fileName, FileMode.Create, FileAccess.Write))
                {
                    fileMemoryStream.WriteTo(file);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Success";
        }

        public async Task<MemoryStream> ReadFileAsMemoryStream(string fileName)
        {
            var streamMemory = new MemoryStream();

            try
            {
                if (!File.Exists(fileName))
                    return streamMemory;

                using (FileStream SourceStream = File.Open(fileName, FileMode.Open))
                {
                    var result = new byte[SourceStream.Length];
                    using (var sr = new StreamReader(SourceStream, Encoding.UTF8))
                    {
                        string content = await sr.ReadToEndAsync();
                        var writer = new StreamWriter(streamMemory);
                        writer.Write(content);
                        writer.Flush();
                        streamMemory.Position = 0;
                        return streamMemory;
                    }
                }
            }
            catch (Exception ex)
            {
                return streamMemory;
            }
        }

        public async Task<string> ReadFileAsText(string fileName)
        {
            string resultJsonText = string.Empty;

            resultJsonText = await File.ReadAllTextAsync(fileName);

            return resultJsonText;
        }

        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize)
            {
                case var _ when fileSize < kilobyte:
                    return $"Less then 1KB";
                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
                default:
                    return "n/a";
            }
        }

        public List<FileProperties> ListFilesInDirectory(
            string directoryName, string excludeExtensions = "")
        {
            List<FileProperties> fileList = null;
            IEnumerable<string> fileListWithProperties = null;

            if (!Directory.Exists(directoryName))
                return fileList;

            if (!string.IsNullOrEmpty(excludeExtensions))
            {
                fileListWithProperties = Directory
                    .GetFiles(directoryName, "*", SearchOption.TopDirectoryOnly)
                    .Where(name => !name.EndsWith(excludeExtensions, StringComparison.OrdinalIgnoreCase))
                    .Select(file => file);
            }
            else
            {
                fileListWithProperties = Directory
                    .GetFiles(directoryName, "*", SearchOption.TopDirectoryOnly);
            }

            if(fileListWithProperties == null || fileListWithProperties.Count() == 0)
            {
                return fileList;
            }

            fileList = new List<FileProperties>();

            foreach (var fullPathName in fileListWithProperties)
            {
                FileProperties fileProperties = new FileProperties();
                FileInfo fileInfo = new FileInfo(fullPathName);
                fileProperties.FullPathName = fullPathName;
                fileProperties.FileName = fileInfo.Name;
                fileProperties.FileSize = fileInfo.Length;
                fileProperties.ParentDirectory = fileInfo.DirectoryName;
                fileProperties.Extension = fileInfo.Extension;
                //fileProperties.Types = TODO;
                fileList.Add(fileProperties);
            }

            return fileList;
        }
    }
}


