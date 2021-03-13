using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic
{
    internal class EntityDetailGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityDetailTemplate.txt");

        private static readonly string FileName = "EntityDetail.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly EntityDetailMethodsAddition dtoDetailMethodsAddition;
        private readonly EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition;
        private readonly EntityDetailToMethodsAddition dtoDetailToMethodsAddition;

        public EntityDetailGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            EntityDetailMethodsAddition dtoDetailMethodsAddition,
            EntityDetailFromMethodsAddition dtoDetailFromMethodsAddition,
            EntityDetailToMethodsAddition dtoDetailToMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dtoDetailMethodsAddition = dtoDetailMethodsAddition;
            this.dtoDetailFromMethodsAddition = dtoDetailFromMethodsAddition;
            this.dtoDetailToMethodsAddition = dtoDetailToMethodsAddition;
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
            IPropertyAdditionOptions propertyAdditionOptionsFrom = RelationAdditionOptions.
                GetPropertyForFrom(options, $"IEnumerable<I{options.EntityNameTo}>", $"{options.EntityNamePluralTo}");

            this.dtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptionsFrom, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.dtoDetailFromMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");

            // To
            IPropertyAdditionOptions propertyAdditionOptionsTo = RelationAdditionOptions.
                GetPropertyForTo(options, $"I{options.EntityNameFrom}", options.EntityNameFrom);

            this.dtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptionsTo, LogicProjectGeneration.DomainFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoDetailToMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName);
        }
    }
}