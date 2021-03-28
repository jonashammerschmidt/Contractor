using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityTemplate.txt");

        private static readonly string FileName = "Entity.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly EntityMethodsAddition dtoMethodsAddition;

        public EntityGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            EntityMethodsAddition dtoMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dtoMethodsAddition = dtoMethodsAddition;
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
            this.dtoMethodsAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions relationSideAdditionOptions = RelationAdditionOptions.
                GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationSideAdditionOptions);

            this.dtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptions, LogicProjectGeneration.DomainFolder, FileName);
            this.dtoMethodsAddition.Add(propertyAdditionOptions, LogicProjectGeneration.DomainFolder, FileName);
        }
    }
}