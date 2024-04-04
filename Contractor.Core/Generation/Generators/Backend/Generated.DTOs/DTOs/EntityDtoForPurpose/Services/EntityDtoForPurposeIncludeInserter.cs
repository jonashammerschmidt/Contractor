using System.Text;
using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    public class EntityDtoForPurposeIncludeInserter
    {
        private readonly IFileSystemClient fileSystemClient;
        private readonly PathService pathService;

        public EntityDtoForPurposeIncludeInserter(IFileSystemClient fileSystemClient, PathService pathService)
        {
            this.fileSystemClient = fileSystemClient;
            this.pathService = pathService;
        }
        
        public void Insert(CustomDto customDto, params string[] paths)
        {
            string filePath = pathService.GetAbsolutePathForBackendGenerated(customDto.Entity, paths);
            string fileData = fileSystemClient.ReadAllText(customDto.Entity, filePath);
            fileData = UsingStatements.Add(fileData, "Microsoft.EntityFrameworkCore");

            StringEditor stringEditor = new StringEditor(fileData);
            stringEditor.MoveToEnd();
            if (!stringEditor.GetLine().Contains("}"))
            {
                stringEditor.PrevThatContains("}");
            }

            stringEditor.PrevThatContains("}");
            stringEditor.InsertNewLine();
            stringEditor.InsertLine($"        public static IQueryable<Ef{customDto.Entity.Name}Dto> AddIncludesToQuery(IQueryable<Ef{customDto.Entity.Name}Dto> ef{customDto.Entity.NamePlural})");
            stringEditor.InsertLine("        {");
            stringEditor.InsertLine($"            return ef{customDto.Entity.NamePlural}");
            stringEditor.InsertLine(CustomDtoIncludeHelper.GetIncludeString(customDto) + ";", 4);
            stringEditor.InsertLine("        }");

            fileSystemClient.WriteAllText(stringEditor.GetText(), filePath);
        }
    }
}