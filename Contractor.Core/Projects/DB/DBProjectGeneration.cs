using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Projects;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects
{
    internal class DBProjectGeneration : IProjectGeneration
    {
        private static readonly string DomainFolder = "dbo\\Tables\\{Domain}";

        private static readonly string TemplateFolder = Folder.Executable + @"\Projects\DB\Templates";
        private static readonly string DbTableTemplateFileName = Path.Combine(TemplateFolder, "TableTemplate.txt");

        private static readonly string DbTableFileName = "Entities.sql";

        private readonly DbProjectFileDomainAddition dbProjectFileDomainAddition;
        private readonly DbProjectFileEntityAddition dbProjectFileEntityAddition;
        private readonly DbTableAddition dbTableAddition;
        private readonly DbTablePropertyAddition dbTablePropertyAddition;
        private readonly DbTableRelationContraintAddition dbTableRelationContraintAddition;
        private readonly PathService pathService;

        public DBProjectGeneration(
            DbProjectFileDomainAddition dbProjectFileDomainAddition,
            DbProjectFileEntityAddition dbProjectFileEntityAddition,
            DbTableAddition dbTableAddition,
            DbTablePropertyAddition dbTablePropertyAddition,
            DbTableRelationContraintAddition dbTableRelationContraintAddition,
            PathService pathService)
        {
            this.dbProjectFileDomainAddition = dbProjectFileDomainAddition;
            this.dbProjectFileEntityAddition = dbProjectFileEntityAddition;
            this.dbTableAddition = dbTableAddition;
            this.dbTablePropertyAddition = dbTablePropertyAddition;
            this.dbTableRelationContraintAddition = dbTableRelationContraintAddition;
            this.pathService = pathService;
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            this.pathService.AddDbDomainFolder(options, DomainFolder);

            this.dbProjectFileDomainAddition.Add(options);
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            string dbTableTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, DbTableTemplateFileName);
            this.dbTableAddition.AddEntityCore(options, DomainFolder, dbTableTemplateFileName, DbTableFileName);

            this.dbProjectFileEntityAddition.Add(options);
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            this.dbTablePropertyAddition.AddProperty(options, DomainFolder, DbTableFileName);
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            this.dbTablePropertyAddition.AddProperty(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                DomainFolder, DbTableFileName);
            this.dbTableRelationContraintAddition.AddContraint(options, DomainFolder, DbTableFileName);
        }
    }
}