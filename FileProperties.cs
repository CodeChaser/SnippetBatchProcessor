using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetBatchProcessor.Models
{
    public class FileProperties
    {
        /// <summary>
        /// This is path and filename
        /// </summary>
        public string FullPathName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ParentDirectory { get; set; }
        public long FileSize { get; set; }
        public List<string> Types { get; set; }
    }
}
