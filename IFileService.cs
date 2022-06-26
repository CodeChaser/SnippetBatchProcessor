using SnippetBatchProcessor.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SnippetBatchProcessor
{
    public interface IFileService
    {
        string WriteMemoryStreamFile(MemoryStream fileMemoryStream, string fileName, string destinationDirectory);
        void UploadFile();
        string SizeConverter(long bytes);
        Task<string> ReadFileAsText(string fileName);
        Task<MemoryStream> ReadFileAsMemoryStream(string fileName);
        List<FileProperties> ListFilesInDirectory(string directoryName, string excludeExtensions = "");
    }
}