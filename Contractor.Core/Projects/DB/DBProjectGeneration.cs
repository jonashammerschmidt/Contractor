using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using Contractor.Core.Jobs.EntityAddition;
using Contractor.Core.Projects.DB.ProjectFile;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class DBProjectGeneration : IProjectGeneration
    {
        
        private static string DomainFolder = "dbo/Tables/{Domain}";

        private static string TemplateFolder = Folder.Executable + @"\Projects\DB\Templates";
        private static string DbTableTemplateFileName = Path.Combine(TemplateFolder, "TableTemplate.txt");

        private static string DbTableFileName = "Entities.sql";

        private DbProjectFileDomainAddition dbProjectFileDomainAddition;
        private DbProjectFileEntityAddition dbProjectFileEntityAddition;
        private DbTableAddition dbTableAddition;
        private DbTablePropertyAddition dbTablePropertyAddition;
        private PathService pathService;

        public DBProjectGeneration(
            DbProjectFileDomainAddition dbProjectFileDomainAddition,
            DbProjectFileEntityAddition dbProjectFileEntityAddition,
            DbTableAddition dbTableAddition,
            DbTablePropertyAddition dbTablePropertyAddition,
            PathService pathService)
        {
            this.dbProjectFileDomainAddition = dbProjectFileDomainAddition;
            this.dbProjectFileEntityAddition = dbProjectFileEntityAddition;
            this.dbTableAddition = dbTableAddition;
            this.dbTablePropertyAddition = dbTablePropertyAddition;
            this.pathService = pathService;
        }

        public void ClearDomain(DomainOptions options)
        {
            this.pathService.DeleteDomainFolder(options, DomainFolder);
        }

        public void AddDomain(DomainOptions options)
        {
            this.pathService.AddDbDomainFolder(options, DomainFolder);

            this.dbProjectFileDomainAddition.Add(options);
        }

        public void AddEntity(EntityOptions options)
        {
            this.dbTableAddition.AddEntityCore(options, DomainFolder, DbTableTemplateFileName, DbTableFileName);

            this.dbProjectFileEntityAddition.Add(options);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.dbTablePropertyAddition.AddProperty(options, DomainFolder, DbTableFileName);
        }
    }
}
