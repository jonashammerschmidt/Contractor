using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Frontend.Interfaces;

namespace Contractor.Core.Generation.Frontend.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_DTOS })]
    public class IEntityDtoExpandedGeneration : ClassGeneration, IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(DTOsProjectGeneration.TemplateFolder, "i-entity-kebab-dto-expanded.template.txt");

        private static readonly string FileName = "i-entity-kebab-dto-expanded.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoRelationAddition frontendDtoRelationAddition;
        private readonly FrontendInterfaceExtender interfaceExtender;

        public IEntityDtoExpandedGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoRelationAddition frontendDtoRelationAddition,
            FrontendInterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.frontendDtoRelationAddition = frontendDtoRelationAddition;
            this.interfaceExtender = interfaceExtender;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToFrontend(entity, DTOsProjectGeneration.DomainFolder, TemplatePath, FileName);
            
            if (entity.HasScope)
            {      
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(entity.ScopeEntity, entity, "I", "Dto");
                
                string toImportStatementPath = $"@generated-app/model/{relationSideTo.OtherEntity.Module.NameKebab}" +
                                               $"/{relationSideTo.OtherEntity.NamePluralKebab}" +
                                               $"/dtos/i-{relationSideTo.OtherEntity.NameKebab}-dto";

                this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, DTOsProjectGeneration.DomainFolder, FileName,
                    $"I{relationSideTo.OtherEntity.Name}Dto", toImportStatementPath);
            }
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

            string toImportStatementPath = $"@generated-app/dtos/{relation.TargetEntity.Module.NameKebab}" +
                $"/{relation.TargetEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.TargetEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, DTOsProjectGeneration.DomainFolder, FileName,
                $"I{relation.TargetEntity.Name}Dto", toImportStatementPath);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "Dto");

            string fromImportStatementPath = $"@generated-app/model/{relation.SourceEntity.Module.NameKebab}" +
                $"/{relation.SourceEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.SourceEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideFrom, DTOsProjectGeneration.DomainFolder, FileName,
                $"I{relation.SourceEntity.Name}Dto", fromImportStatementPath);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "Dto");

            string toImportStatementPath = $"@generated-app/model/{relation.TargetEntity.Module.NameKebab}" +
                $"/{relation.TargetEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.TargetEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, DTOsProjectGeneration.DomainFolder, FileName,
                $"I{relation.TargetEntity.Name}Dto", toImportStatementPath);
        }

        public void AddInterface(GenerationOptions options, Interface interfaceItem)
        {
            foreach (var module in options.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    var entityInterfaceCompatibility = EntityInterfaceCompatibilityChecker.IsInterfaceCompatible(entity, interfaceItem);
                    if (entityInterfaceCompatibility == EntityInterfaceCompatibility.DtoExpanded)
                    {
                        this.interfaceExtender.AddInterface(
                            entity,
                            interfaceItem.Name,
                            DTOsProjectGeneration.DomainFolder,
                            FileName);
                    }
                }
            }
        }
    }
}