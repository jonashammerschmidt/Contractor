using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Frontend.Interfaces
{
    public class FrontendInterfaceExtender
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public FrontendInterfaceExtender(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void AddInterface(Entity entity, string interfaceName, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForFrontend(entity, paths);
            string fileData = fileSystemClient.ReadAllText(entity, filePath);

            string interfacesPath = $"src/app/model/_interfaces/i-{interfaceName.ToKebab()}";
            fileData = ImportStatements.Add(fileData, "I" + interfaceName, interfacesPath);

            StringEditor stringEditor = new StringEditor(fileData);
            if (!stringEditor.GetLine().Contains("export"))
            {
                stringEditor.NextThatStartsWith($"export");
            }
            
            string classDefinitionLine = stringEditor.GetLine();
            
            if (classDefinitionLine.Contains(" extends "))
            {
                stringEditor.SetLine(classDefinitionLine.Substring(0, classDefinitionLine.Length - 2) + ", I" + interfaceName + " {");
            }
            else
            {
                stringEditor.SetLine(classDefinitionLine.Substring(0, classDefinitionLine.Length - 2) + " extends I" + interfaceName + " {");
            }
            fileData = stringEditor.GetText();
            
            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}