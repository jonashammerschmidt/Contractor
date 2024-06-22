using System;
using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Helpers;

namespace Contractor.Core.Generation.Frontend.Interfaces
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_GENERATED, ClassGenerationTag.BACKEND_GENERATED_INTERFACES })]
    public class FrontendInterfaceGeneration : IInterfaceGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(FrontendInterfacesProjectGeneration.TemplateFolder, "i-interface.template.txt");

        private static readonly string FileName = "i-interface-name.ts";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly FrontendDtoPropertyAddition propertyAddition;
        private readonly FrontendDtoRelationAddition relationAddition;
        private readonly FrontendInterfaceExtender frontendInterfaceExtender;

        public FrontendInterfaceGeneration(
            EntityCoreAddition entityCoreAddition,
            FrontendDtoPropertyAddition propertyAddition,
            FrontendDtoRelationAddition relationAddition,
            FrontendInterfaceExtender frontendInterfaceExtender)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
            this.frontendInterfaceExtender = frontendInterfaceExtender;
        }

        public void AddInterface(GenerationOptions options, Interface interfaceItem)
        {
            Entity compatibleEntity = FindCompatibleEntity(options, interfaceItem);

            if (compatibleEntity is null)
            {
                Console.WriteLine($"--- WARNING : Skipping interface generation for i-{interfaceItem.Name.ToKebab()}, because no compatible dto was found.");
            }
            else
            {
                this.entityCoreAddition.AddInterfaceToFrontend(
                    interfaceItem,
                    compatibleEntity,
                    FrontendInterfacesProjectGeneration.DomainFolder,
                    TemplatePath,
                    FileName.Replace("interface-name", interfaceItem.Name.ToKebab()));

                foreach (var extendedInterface in interfaceItem.ExtendedInterfaces)
                {
                    this.frontendInterfaceExtender.AddInterface(
                        compatibleEntity,
                        extendedInterface.Name,
                        FrontendInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("interface-name", interfaceItem.Name.ToKebab()));
                }

                foreach (var interfaceProperty in interfaceItem.Properties)
                {
                    this.propertyAddition.AddPropertyToFrontendFile(
                        compatibleEntity.FindPropertyIncludingIds(interfaceProperty.Name), 
                        FrontendInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("interface-name", interfaceItem.Name.ToKebab()));
                }

                foreach (var interfaceRelation in interfaceItem.Relations)
                {
                    var relation = options.FindRelation(compatibleEntity, interfaceRelation.PropertyName ?? interfaceRelation.TargetEntityName);
                    RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "Dto");
                
                    string toImportStatementPath = $"@generated-app/dtos/{relation.TargetEntity.Module.NameKebab}" +
                                                   $"/{relation.TargetEntity.NamePluralKebab}" +
                                                   $"/dtos/i-{relation.TargetEntity.NameKebab}-dto";
                
                    this.relationAddition.AddPropertyToDTO(
                        relationSideTo,
                        FrontendInterfacesProjectGeneration.DomainFolder,
                        FileName.Replace("interface-name", interfaceItem.Name.ToKebab()),
                        $"I{relation.TargetEntity.Name}Dto", toImportStatementPath);
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