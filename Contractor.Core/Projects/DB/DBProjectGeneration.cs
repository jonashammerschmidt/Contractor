using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using Contractor.Core.Projects.DB.ProjectFile;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Template.Logic
{
    public class DBProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = "dbo/Tables/{Domain}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\DB\Templates";
        private static readonly string DbTableTemplateFileName = Path.Combine(TemplateFolder, "TableTemplate.txt");

        private static readonly string DbTableFileName = "Entities.sql";

        private readonly DbProjectFileDomainAddition dbProjectFileDomainAddition;
        private readonly DbProjectFileEntityAddition dbProjectFileEntityAddition;
        private readonly DbTableAddition dbTableAddition;
        private readonly DbTablePropertyAddition dbTablePropertyAddition;
        private readonly PathService pathService;

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

        public void AddDomain(DomainOptions options)
        {
            this.pathService.AddDbDomainFolder(options, DomainFolder);

            this.dbProjectFileDomainAddition.Add(options);
        }

        public void AddEntity(EntityOptions options)
        {
            string dbTableTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, DbTableTemplateFileName);
            this.dbTableAddition.AddEntityCore(options, DomainFolder, dbTableTemplateFileName, DbTableFileName);

            this.dbProjectFileEntityAddition.Add(options);
        }

        public void AddProperty(PropertyOptions options)
        {
            this.dbTablePropertyAddition.AddProperty(options, DomainFolder, DbTableFileName);
        }
    }
}