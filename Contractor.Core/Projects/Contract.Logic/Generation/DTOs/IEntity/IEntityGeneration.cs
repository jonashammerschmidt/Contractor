﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Contract.Logic
{
    internal class IEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(ContractLogicProjectGeneration.TemplateFolder, "IEntityTemplate.txt");

        private static readonly string FileName = "IEntity.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public IEntityGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.dtoAddition.AddDto(options, ContractLogicProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, ContractLogicProjectGeneration.DomainFolder, FileName, true);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id"),
                ContractLogicProjectGeneration.DomainFolder, FileName, true);
        }
    }
}