using Contractor.Core.Options;
using Contractor.Core.Tools;

namespace Contractor.Core.Projects.Database
{
    [ClassGenerationTags(new[] { ClassGenerationTag.DATABASE })]
    internal class DbProjectFileGeneration : ClassGeneration
    {
        private readonly DbProjectFileDomainAddition dbProjectFileDomainAddition;
        private readonly DbProjectFileEntityAddition dbProjectFileEntityAddition;

        public DbProjectFileGeneration(
            DbProjectFileDomainAddition dbProjectFileDomainAddition,
            DbProjectFileEntityAddition dbProjectFileEntityAddition)
        {
            this.dbProjectFileDomainAddition = dbProjectFileDomainAddition;
            this.dbProjectFileEntityAddition = dbProjectFileEntityAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
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

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}