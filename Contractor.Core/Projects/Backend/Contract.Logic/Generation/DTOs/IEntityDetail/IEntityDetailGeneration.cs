﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Contract.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_CONTRACT_LOGIC })]
    internal class IEntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityDetailTemplate.txt");

        private static readonly string FileName = "IEntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;
        private readonly DtoRelationAddition relationAddition;

        public IEntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition,
            DtoRelationAddition relationAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
            this.relationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, ContractLogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.propertyAddition.AddPropertyToDTO(property, ContractLogicProjectGeneration.DtoFolder, FileName, true);
        }

        protected override void Add1ToNRelation(Relation1ToN relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "IEnumerable<I", ">");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.Entity.Module.Name}.{relationSideTo.Entity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.Entity.Module.Name}.{relationSideFrom.Entity.NamePlural}");
        }

        protected override void AddOneToOneRelation(Relation1To1 relation)
        {
            RelationSide relationSideFrom = RelationSide.FromObjectRelationEndFrom(relation, "I", "");
            RelationSide relationSideTo = RelationSide.FromObjectRelationEndTo(relation, "I", "");

            // From
            this.relationAddition.AddRelationToDTO(relationSideFrom, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideFrom.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideTo.Entity.Module.Name}.{relationSideTo.Entity.NamePlural}");

            // To
            this.relationAddition.AddRelationToDTO(relationSideTo, ContractLogicProjectGeneration.DtoFolder, FileName, true,
                $"{relationSideTo.Entity.Module.Options.Paths.ProjectName}.Contract.Logic.Modules.{relationSideFrom.Entity.Module.Name}.{relationSideFrom.Entity.NamePlural}");
        }
    }
}