using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnippetBatchProcessor
{
    class Program
    {

        static async Task Main(string[] args)
        {
            await APICrudFileGenerator.GenerateAPICrudFile(string.Empty);
        }
    }
}
