using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
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
            this.dtoMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions relationSideAdditionOptions = RelationAdditionOptions.
                GetPropertyForTo(options, "Guid");

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationSideAdditionOptions);

            this.dtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptions, LogicProjectGeneration.DtoFolder, FileName);
            this.dtoMethodsAddition.Edit(propertyAdditionOptions, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}