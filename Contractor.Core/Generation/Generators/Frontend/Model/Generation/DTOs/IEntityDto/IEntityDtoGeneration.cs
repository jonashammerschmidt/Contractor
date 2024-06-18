using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;

namespace Contractor.Core.Generation.Frontend.Model
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_MODEL })]
    public class IEntityDtoGeneration : ClassGeneration, IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ModelProjectGeneration.TemplateFolder, "i-entity-kebab-dto.template.txt");

        private static readonly string FileName = "i-entity-kebab-dto.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoPropertyAddition frontendDtoPropertyAddition;
        private readonly FrontendInterfaceExtender interfaceExtender;

        public IEntityDtoGeneration(
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
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.entityCoreAddition.AddEntityToFrontend(entity, ModelProjectGeneration.DomainDtoFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
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
                    if (entityInterfaceCompatibility == EntityInterfaceCompatibility.Dto)
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