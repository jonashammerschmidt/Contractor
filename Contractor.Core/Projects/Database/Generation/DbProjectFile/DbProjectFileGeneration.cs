using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database
{
    internal class DbProjectFileGeneration : ClassGeneration
    {
        private readonly DbProjectFileDomainAddition dbProjectFileDomainAddition;
        private readonly DbProjectFileEntityAddition dbProjectFileEntityAddition;
        private readonly PathService pathService;

        public DbProjectFileGeneration(
            DbProjectFileDomainAddition dbProjectFileDomainAddition,
            DbProjectFileEntityAddition dbProjectFileEntityAddition,
            PathService pathService)
        {
            this.dbProjectFileDomainAddition = dbProjectFileDomainAddition;
            this.dbProjectFileEntityAddition = dbProjectFileEntityAddition;
            this.pathService = pathService;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
            this.pathService.AddDbDomainFolder(options, DBProjectGeneration.DomainFolder);

            this.dbProjectFileDomainAddition.Add(options);
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dbProjectFileEntityAddition.Add(options);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}