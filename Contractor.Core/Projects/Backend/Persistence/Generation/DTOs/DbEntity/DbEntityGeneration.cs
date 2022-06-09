using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Persistence
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_PERSISTENCE })]
    internal class DbEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PersistenceProjectGeneration.TemplateFolder, "DbEntityTemplate.txt");

        private static readonly string FileName = "DbEntity.cs";

        private readonly DbEntityMethodsAddition dbDtoMethodsAddition;
        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition propertyAddition;

        public DbEntityGeneration(
            DbEntityMethodsAddition dbDtoMethodsAddition,
            DtoAddition dtoAddition,
            DtoPropertyAddition propertyAddition)
        {
            this.dbDtoMethodsAddition = dbDtoMethodsAddition;
            this.dtoAddition = dtoAddition;
            this.propertyAddition = propertyAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            string templatePath = TemplateFileName.GetFileNameForEntityAddition(entity, TemplatePath);
            this.dtoAddition.AddDto(entity, PersistenceProjectGeneration.DtoFolder, templatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.propertyAddition.AddPropertyToDTO(options, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoMethodsAddition.Edit(options, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            IRelationSideAdditionOptions relationSideAdditionOptions =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid");
            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationSideAdditionOptions);

            this.propertyAddition.AddPropertyToDTO(propertyAdditionOptions, PersistenceProjectGeneration.DtoFolder, FileName);
            this.dbDtoMethodsAddition.Edit(propertyAdditionOptions, PersistenceProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}