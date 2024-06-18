using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.Core.Generation.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    public class IEntityDtoExpandedGeneration : ClassGeneration, IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-dto-expanded.template.txt");

        private static readonly string FileName = "i-entity-kebab-dto-expanded.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendDtoRelationAddition frontendDtoRelationAddition;
        private readonly FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition;
        private readonly FrontendInterfaceExtender interfaceExtender;

        public IEntityDtoExpandedGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendDtoRelationAddition frontendDtoRelationAddition,
            FrontendDtoPropertyMethodAddition frontendDtoPropertyMethodAddition,
            FrontendInterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.frontendDtoRelationAddition = frontendDtoRelationAddition;
            this.frontendDtoPropertyMethodAddition = frontendDtoPropertyMethodAddition;
            this.interfaceExtender = interfaceExtender;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToFrontend(entity, ModelProjectGeneration.DomainDtoFolder, TemplatePath, FileName);
            
            if (entity.HasScope)
            {      
                RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(entity.ScopeEntity, entity, "I", "Dto");
                
                string toImportStatementPath = $"src/app/model/{relationSideTo.OtherEntity.Module.NameKebab}" +
                                               $"/{relationSideTo.OtherEntity.NamePluralKebab}" +
                                               $"/dtos/i-{relationSideTo.OtherEntity.NameKebab}-dto";

                this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainDtoFolder, FileName,
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

            string toImportStatementPath = $"src/app/model/{relation.TargetEntity.Module.NameKebab}" +
                $"/{relation.TargetEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.TargetEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainDtoFolder, FileName,
                $"I{relation.TargetEntity.Name}Dto", toImportStatementPath);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "Dto");

            string fromImportStatementPath = $"src/app/model/{relation.SourceEntity.Module.NameKebab}" +
                $"/{relation.SourceEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.SourceEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideFrom, ModelProjectGeneration.DomainDtoFolder, FileName,
                $"I{relation.SourceEntity.Name}Dto", fromImportStatementPath);
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "Dto");

            string toImportStatementPath = $"src/app/model/{relation.TargetEntity.Module.NameKebab}" +
                $"/{relation.TargetEntity.NamePluralKebab}" +
                $"/dtos/i-{relation.TargetEntity.NameKebab}-dto";

            this.frontendDtoRelationAddition.AddPropertyToDTO(relationSideTo, ModelProjectGeneration.DomainDtoFolder, FileName,
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
                            ModelProjectGeneration.DomainDtoFolder,
                            FileName);
                    }
                }
            }
        }
    }
}