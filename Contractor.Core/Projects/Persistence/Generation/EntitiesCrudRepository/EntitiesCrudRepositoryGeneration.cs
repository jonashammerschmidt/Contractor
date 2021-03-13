using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Persistence
{
    internal class EntitiesCrudRepositoryGeneration : ClassGeneration
    {
        public static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "EntitiesCrudRepositoryTemplate.txt");

        public static readonly string FileName = "EntitiesCrudRepository.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoFromRepositoryIncludeAddition dtoFromRepositoryIncludeAddition;
        private readonly DtoToRepositoryIncludeAddition dtoToRepositoryIncludeAddition;

        public EntitiesCrudRepositoryGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoFromRepositoryIncludeAddition dtoFromRepositoryIncludeAddition,
            DtoToRepositoryIncludeAddition dtoToRepositoryIncludeAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoFromRepositoryIncludeAddition = dtoFromRepositoryIncludeAddition;
            this.dtoToRepositoryIncludeAddition = dtoToRepositoryIncludeAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(options, TemplatePath);
            this.entityCoreAddition.AddEntityCore(options, LogicTestsProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            this.dtoFromRepositoryIncludeAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);

            // To
            this.dtoToRepositoryIncludeAddition.Add(options, LogicTestsProjectGeneration.DomainFolder, FileName);
        }
    }
}