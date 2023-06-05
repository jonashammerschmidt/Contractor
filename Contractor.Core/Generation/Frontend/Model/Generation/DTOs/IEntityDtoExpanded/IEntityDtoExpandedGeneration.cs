using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class IEntityDtoExpandedGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-dto-expanded.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-dto-expanded.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoRelationAddition frontendDtoRelationAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;

        public IEntityDtoExpandedGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoRelationAddition frontendDtoRelationAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoRelationAddition = frontendDtoRelationAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToFrontend(entity, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "Dto");

            string toImportStatementPath = $"src/app/model/{relation.EntityFrom.Module.NameKebab}" +
                $"/{relation.EntityFrom.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityFrom.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityFrom.Name}Dto", toImportStatementPath);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "Dto");

            string fromImportStatementPath = $"src/app/model/{relation.EntityTo.Module.NameKebab}" +
                $"/{relation.EntityTo.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityTo.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideFrom, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityTo.Name}Dto", fromImportStatementPath);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "Dto");

            string toImportStatementPath = $"src/app/model/{relation.EntityFrom.Module.NameKebab}" +
                $"/{relation.EntityFrom.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityFrom.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityFrom.Name}Dto", toImportStatementPath);
        }
    }
}