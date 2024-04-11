using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class InterfaceExtender
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public InterfaceExtender(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void AddInterfaceToClass(Entity entity, string interfaceName, params string[] paths)
        {
            AddInterface(entity, interfaceName, "class", paths);
        }
        
        public void AddInterfaceToInterface(Entity entity, string interfaceName, params string[] paths)
        {
            AddInterface(entity, interfaceName, "interface", paths);
        }

        private void AddInterface(Entity entity, string interfaceName, string type, string[] paths)
        {            
            string filePath = pathService.GetAbsolutePathForBackendGenerated(entity, paths);
            string fileData = fileSystemClient.ReadAllText(entity, filePath);

            string interfacesNamespace = $"{entity.Module.Options.Paths.GeneratedProjectName}.Interfaces";
            fileData = UsingStatements.Add(fileData, interfacesNamespace);

            StringEditor stringEditor = new StringEditor(fileData);
            
            stringEditor.NextThatContains("public " + type);

            string classDefinitionLine = stringEditor.GetLine();
            
            if (classDefinitionLine.Contains(":"))
            {
                stringEditor.SetLine(classDefinitionLine + ", I" + interfaceName);
            }
            else
            {
                stringEditor.SetLine(classDefinitionLine + " : I" + interfaceName);
            }
            fileData = stringEditor.GetText();
            
            fileSystemClient.WriteAllText(fileData, filePath);
        }
    }
}