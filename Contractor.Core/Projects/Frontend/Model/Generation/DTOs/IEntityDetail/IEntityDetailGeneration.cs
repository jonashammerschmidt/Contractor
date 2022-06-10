using Contractor.Core.Helpers;
using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    internal class IEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-detail.template.txt");

        private static readonly string FileName = "dtos\\i-entity-kebab-detail.ts";

        private readonly FrontendEntityAddition frontendEntityAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoRelationAddition frontendDtoRelationAddition;

        public IEntityDetailGeneration(
            FrontendEntityAddition frontendEntityAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoRelationAddition frontendDtoRelationAddition)
        {
            this.frontendEntityAddition = frontendEntityAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoRelationAddition = frontendDtoRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.frontendEntityAddition.AddEntity(entity, ModelProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.frontendDtoPropertyAddition.AddPropertyToDTO(property, ModelProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "[]");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            string fromImportStatementPath = $"src/app/model/{relation.EntityTo.Module.NameKebab}" +
                $"/{relation.EntityTo.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityTo.NameKebab}";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideFrom, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityTo.Name}", fromImportStatementPath);

            // To
            string toImportStatementPath = $"src/app/model/{relation.EntityFrom.Module.NameKebab}" +
                $"/{relation.EntityFrom.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityFrom.NameKebab}";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityFrom.Name}", toImportStatementPath);
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            string fromImportStatementPath = $"src/app/model/{relation.EntityTo.Module.NameKebab}" +
                $"/{relation.EntityTo.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityTo.NameKebab}";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideFrom, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityTo.Name}", fromImportStatementPath);

            // To
            string toImportStatementPath = $"src/app/model/{relation.EntityFrom.Module.NameKebab}" +
                $"/{relation.EntityFrom.NamePluralKebab}" +
                $"/dtos/i-{relation.EntityFrom.NameKebab}";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainFolder, FileName,
                $"I{relation.EntityFrom.Name}", toImportStatementPath);
        }
    }
}