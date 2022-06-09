﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class DbEntityUpdateGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "DbEntityUpdateTemplate.txt");

        private static readonly string FileName = "DbEntityUpdate.cs";

        private readonly DtoAddition dtoAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DbEntityUpdateMethodsAddition dbEntityUpdateMethodsAddition;

        public DbEntityUpdateGeneration(
            DtoAddition dtoAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DbEntityUpdateMethodsAddition dbEntityUpdateMethodsAddition)
        {
            this.dtoAddition = dtoAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dbEntityUpdateMethodsAddition = dbEntityUpdateMethodsAddition;
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
            this.dbEntityUpdateMethodsAddition.Edit(options, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IRelationSideAdditionOptions relationSideAdditionOptions = RelationAdditionOptions.
                GetPropertyForTo(options, "Guid");

            PropertyAdditionOptions propertyAdditionOptions = new PropertyAdditionOptions(relationSideAdditionOptions);

            this.dtoPropertyAddition.AddPropertyToDTO(propertyAdditionOptions, LogicProjectGeneration.DtoFolder, FileName);
            this.dbEntityUpdateMethodsAddition.Edit(propertyAdditionOptions, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            this.Add1ToNRelation(options);
        }
    }
}