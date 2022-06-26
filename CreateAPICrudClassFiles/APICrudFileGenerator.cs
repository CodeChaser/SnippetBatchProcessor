using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SnippetBatchProcessor
{
    public static class APICrudFileGenerator
    {
        private static FileService _fileService;

        private static string templateFolderName = Environment.CurrentDirectory + @"\CreateAPICrudClassFiles\Templates\";

        public async static Task<string> GenerateAPICrudFile(string json)
        {
            _fileService = new FileService();

            var fileFullNames = new List<(string DirectoryName, string APIName)>
            {
                       (@"D:\Projects\PortalUsersAPI\src\FunctionApps\v1\API\Projects",
                        "Project"),
                       (@"D:\Projects\PortalUsersAPI\src\FunctionApps\v1\API\Organizations",
                        "Organization"),
            };

            try
            {
                foreach (var fileFullName in fileFullNames)
                {
                    await CreateAPICrudFiles(fileFullName.DirectoryName, fileFullName.APIName);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return default;
        }

        static async Task CreateAPICrudFiles(string directoryName, string apiName)
        {
            string uriPath = string.Empty;

            var replacementMetaData = new List<(string ReplacementID, string ReplacementString)>
            {
                ("$EntityTableRowType$",apiName),
                ("$MissingRowKeyMessage$"," $\"Missing rowkey from URI path: [{uriPath}]\" "),
                ("$BodyRequest$"," \"{\" + $@\"'RowKey': '{rowkey}'\" + \"}\" ")
            };

            await _fileService.WriteTextFile(directoryName, "api_Delete" + apiName + ".cs", 
                GetTemplate("APIDelete",apiName, replacementMetaData));

            await _fileService.WriteTextFile(directoryName, "api_Get" + apiName + "s.cs",
                GetTemplate("APIGet", apiName, replacementMetaData));
            await _fileService.WriteTextFile(directoryName, "api_Get" + apiName + "sByPostQuery.cs",
                GetTemplate("APIQuery", apiName, replacementMetaData));
            await _fileService.WriteTextFile(directoryName, "api_Post" + apiName + "Upsert.cs",
                GetTemplate("APIUpsert", apiName, replacementMetaData));
        }

        static string GetTemplate(
            string templateType, 
            string apiName, List<(string ReplacementID,string ReplacementString)> replacementMetaData)
        {

            var templateName =  templateFolderName + templateType + ".xml";

            XmlDocument xmlDocument = new();

            xmlDocument.Load(templateName);

            XmlCDataSection cDataNode = (XmlCDataSection)(xmlDocument.SelectSingleNode("code").ChildNodes[0]);

            var codeResult = cDataNode.Data;

            foreach (var replacementData in replacementMetaData)
            {
                codeResult = codeResult.Replace(replacementData.ReplacementID, replacementData.ReplacementString);
            }

            return codeResult;
        }
    }
}
