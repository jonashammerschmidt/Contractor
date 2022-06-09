using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntityListItemGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntityListItemTemplate.txt");

        private static readonly string FileName = "EntityListItem.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DtoRelationAddition relationAddition;
        private readonly EntityListItemMethodsAddition dtoListItemMethodsAddition;
        private readonly EntityListItemToMethodsAddition dtoListItemToMethodsAddition;
        private readonly EntityListItemFromOneToOneMethodsAddition entityListItemFromOneToOneMethodsAddition;

        public EntityListItemGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DtoRelationAddition relationAddition,
            EntityListItemMethodsAddition dtoListItemMethodsAddition,
            EntityListItemToMethodsAddition dtoListItemToMethodsAddition,
            EntityListItemFromOneToOneMethodsAddition entityListItemFromOneToOneMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.relationAddition = relationAddition;
            this.dtoListItemMethodsAddition = dtoListItemMethodsAddition;
            this.dtoListItemToMethodsAddition = dtoListItemToMethodsAddition;
            this.entityListItemFromOneToOneMethodsAddition = entityListItemFromOneToOneMethodsAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.dtoAddition.AddDto(entity, LogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.dtoPropertyAddition.AddPropertyToDTO(options, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoListItemMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions relationAdditionOptionsTo = RelationAdditionOptions.
                GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoListItemToMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // From
            IRelationSideAdditionOptions relationAdditionOptionsFrom = RelationAdditionOptions.
                GetPropertyForFrom(options, $"I{options.EntityNameTo}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsFrom, LogicProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainTo}.{options.EntityNamePluralTo}");
            this.entityListItemFromOneToOneMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);

            // To
            IRelationSideAdditionOptions relationAdditionOptionsTo = RelationAdditionOptions.
                GetPropertyForTo(options, $"I{options.EntityNameFrom}");

            this.relationAddition.AddRelationToDTO(relationAdditionOptionsTo, LogicProjectGeneration.DtoFolder, FileName,
                $"{options.ProjectName}.Contract.Logic.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}");
            this.dtoListItemToMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);
        }
    }
}