using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityDetailTemplate.txt");

        private static readonly string FileName = "EntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityDetailMethodsAddition dtoDetailMethodsAddition;
        private readonly EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition;
        private readonly EntityDetailToMethodsAddition dtoDetailToMethodsAddition;
        private readonly EntityDetailFromOneToOneMethodsAddition entityDetailFromOneToOneMethodsAddition;

        public EntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DtoRelationAddition relationAddition,
            EntityDetailMethodsAddition dtoDetailMethodsAddition,
            EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition,
            EntityDetailToMethodsAddition dtoDetailToMethodsAddition,
            EntityDetailFromOneToOneMethodsAddition entityDetailFromOneToOneMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.relationAddition = relationAddition;
            this.dtoDetailMethodsAddition = dtoDetailMethodsAddition;
            this.dtoDetailFromMethodsAddition = dtoDetailFromMethodsAddition;
            this.dtoDetailToMethodsAddition = dtoDetailToMethodsAddition;
            this.entityDetailFromOneToOneMethodsAddition = entityDetailFromOneToOneMethodsAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, LogicProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.dtoPropertyAddition.AddPropertyToDTO(options, LogicProjectGeneration.DomainFolder, FileName);
            this.dtoDetailMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions relationAdditionOptionsFrom = RelationAdditionOptions.
                GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsFrom, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.dtoDetailFromMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions relationAdditionOptionsTo = RelationAdditionOptions.
                GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsTo, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoDetailToMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions relationAdditionOptionsFrom = RelationAdditionOptions.
                GetPropertyForFrom(options, $"I{options.EntityNameTo}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsFrom, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.entityDetailFromOneToOneMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IRelationSideAdditionOptions relationAdditionOptionsTo = RelationAdditionOptions.
                GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsTo, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoDetailToMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName);
        }
    }
}