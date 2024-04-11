using System;
using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.Interfaces;

namespace Contractor.Core.Generation.Backend.Generated.DTOs
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_INTERFACES })]
    public class InterfaceGeneration : IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(GeneratedInterfacesProjectGeneration.TemplateFolder, "InterfaceTemplate.txt");

        private static readonly string FileName = "IInterfaceName.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoInterfacePropertyAddition dtoInterfacePropertyAddition;
        private readonly DtoRelationAddition dtoRelationAddition;
        private readonly InterfaceExtender interfaceExtender;

        public InterfaceGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoInterfacePropertyAddition dtoInterfacePropertyAddition,
            DtoRelationAddition dtoRelationAddition,
            InterfaceExtender interfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoInterfacePropertyAddition = dtoInterfacePropertyAddition;
            this.dtoRelationAddition = dtoRelationAddition;
            this.interfaceExtender = interfaceExtender;
        }

        public void AddInterface(GenerationOptions options, Interface interfaceItem)
        {
            Entity compatibleEntity = FindCompatibleEntity(options, interfaceItem);

            if (compatibleEntity is null)
            {
                Console.WriteLine($"--- WARNING : Skipping interface generation for I{interfaceItem.Name}, because no compatible dto was found.");
            }
            else
            {
                this.entityCoreAddition.AddInterfaceToBackendGenerated(
                    interfaceItem,
                    compatibleEntity,
                    GeneratedInterfacesProjectGeneration.DomainFolder,
                    TemplatePath,
                    FileName.Replace("InterfaceName", interfaceItem.Name));

                foreach (var extendedInterface in interfaceItem.ExtendedInterfaces)
                {
                    this.interfaceExtender.AddInterfaceToInterface(
                        compatibleEntity,
                        extendedInterface.Name,
                        GeneratedInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("InterfaceName", interfaceItem.Name));
                }

                foreach (var interfaceProperty in interfaceItem.Properties)
                {
                    this.dtoInterfacePropertyAddition.AddPropertyToBackendGeneratedFile(
                        compatibleEntity.FindPropertyIncludingIds(interfaceProperty.Name), 
                        GeneratedInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("InterfaceName", interfaceItem.Name));
                }

                foreach (var interfaceRelation in interfaceItem.Relations)
                {
                    var relation = options.FindRelation(compatibleEntity, interfaceRelation.PropertyName ?? interfaceRelation.TargetEntityName);
                    RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "", "Dto");

                    this.dtoRelationAddition.AddRelationToDTOForBackendGenerated(
                        relationSideTo,
                        GeneratedInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("InterfaceName", interfaceItem.Name),
                        true,
                        $"{relationSideTo.Entity.Module.Options.Paths.GeneratedProjectName}.Modules.{relationSideTo.OtherEntity.Module.Name}.{relationSideTo.OtherEntity.NamePlural}");
                }
            }
        }

        private Entity FindCompatibleEntity(GenerationOptions options, Interface interfaceItem)
        {
            foreach (var module in options.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    if (EntityInterfaceCompatibilityChecker.IsInterfaceCompatible(entity, interfaceItem) != EntityInterfaceCompatibility.None)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }
    }
}