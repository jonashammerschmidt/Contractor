using System;
using System.Linq;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class ClassInterfaceExtender
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public ClassInterfaceExtender(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void AddInterfaceToClass(Entity entity, string interfaceName, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(entity, paths);
            string fileData = fileSystemClient.ReadAllText(entity, filePath);

            StringEditor stringEditor = new StringEditor(fileData);
            
            stringEditor.NextThatContains("public class");

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