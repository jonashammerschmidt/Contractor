using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Frontend.Interfaces;

namespace Contractor.Core.Generation.Frontend.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_DTOS })]
    public class IEntityDtoDataGeneration : ClassGeneration, IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(DTOsProjectGeneration.TemplateFolder, "i-entity-kebab-dto-data.template.txt");

        private static readonly string FileName = "i-entity-kebab-dto-data.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendInterfaceExtender interfaceExtender;

        public IEntityDtoDataGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoPropertyAddition frontendDtoPropertyAddition,
            FrontendInterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.frontendDtoPropertyAddition = frontendDtoPropertyAddition;
            this.interfaceExtender = interfaceExtender;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToFrontend(entity, DTOsProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.frontendDtoPropertyAddition.AddPropertyToFrontendFile(property, DTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.frontendDtoPropertyAddition.AddPropertyToFrontendFile(relationSideTo, DTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
            this.Add1ToNRelationSideFrom(new Relation1ToN(relation));
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
        }

        public void AddInterface(GenerationOptions options, Interface interfaceItem)
        {
            foreach (var module in options.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    var entityInterfaceCompatibility = EntityInterfaceCompatibilityChecker.IsInterfaceCompatible(entity, interfaceItem);
                    if (entityInterfaceCompatibility == EntityInterfaceCompatibility.DtoData)
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