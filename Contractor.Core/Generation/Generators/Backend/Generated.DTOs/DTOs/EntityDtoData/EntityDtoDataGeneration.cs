using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_DTOS })]
    public class EntityDtoDataGeneration : ClassGeneration, IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoDataTemplate.txt");
        private static readonly string TemplatePathEmpty =
            Path.Combine(GeneratedDTOsProjectGeneration.TemplateFolder, "EntityDtoDataTemplate-Empty.txt");

        private static readonly string FileName = "EntityDtoData.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly ApiDtoPropertyAddition apiPropertyAddition;
        private readonly EntityDtoDataMethodsAddition entityDtoDataMethodsAddition;
        private readonly ClassInterfaceExtender classInterfaceExtender;

        public EntityDtoDataGeneration(
            EntityCoreAddition entityCoreAddition,
            ApiDtoPropertyAddition apiPropertyAddition,
            EntityDtoDataMethodsAddition entityDtoDataMethodsAddition,
            ClassInterfaceExtender classInterfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.apiPropertyAddition = apiPropertyAddition;
            this.entityDtoDataMethodsAddition = entityDtoDataMethodsAddition;
            this.classInterfaceExtender = classInterfaceExtender;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplatePath;
            if (!entity.HasPropertiesOrRelations()) {
                templatePath = TemplatePathEmpty;
            }

            this.entityCoreAddition.AddEntityToBackendGenerated(entity, GeneratedDTOsProjectGeneration.DomainFolder, templatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            if (property.IsHidden)
            {
                return;
            }

            this.apiPropertyAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
            this.entityDtoDataMethodsAddition.AddPropertyToBackendGeneratedFile(property, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSide = RelationSide.FromGuidRelationEndTo(relation);
            this.apiPropertyAddition.AddPropertyToBackendGeneratedFile(relationSide, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
            this.entityDtoDataMethodsAddition.AddPropertyToBackendGeneratedFile(relationSide, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
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
                        this.classInterfaceExtender.AddInterfaceToClass(entity, interfaceItem.Name, GeneratedDTOsProjectGeneration.DomainFolder, FileName);
                    }
                }
            }
        }
    }
}