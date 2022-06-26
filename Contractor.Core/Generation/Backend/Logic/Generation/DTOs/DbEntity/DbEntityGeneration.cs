using Contractor.Core.BaseClasses;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Generation.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class DbEntityGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "DbEntityTemplate.txt");

        private static readonly string FileName = "DbEntity.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly DtoPropertyAddition dtoPropertyAddition;
        private readonly DbEntityMethodsAddition dbEntityMethodsAddition;
        private readonly DtoRelationAddition dtoRelationAddition;

        public DbEntityGeneration(
            EntityCoreAddition entityCoreAddition,
            DtoPropertyAddition dtoPropertyAddition,
            DbEntityMethodsAddition dbEntityMethodsAddition,
            DtoRelationAddition relationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.dtoPropertyAddition = dtoPropertyAddition;
            this.dbEntityMethodsAddition = dbEntityMethodsAddition;
            this.dtoRelationAddition = relationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(Entity entity)
        {
            this.entityCoreAddition.AddEntityToBackend(entity, LogicProjectGeneration.DtoFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(Property property)
        {
            this.dtoPropertyAddition.AddPropertyToBackendFile(property, LogicProjectGeneration.DtoFolder, FileName);
            this.dbEntityMethodsAddition.AddPropertyToBackendFile(property, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void Add1ToNRelationSideFrom(Relation1ToN relation)
        {
        }

        protected override void Add1ToNRelationSideTo(Relation1ToN relation)
        {
            RelationSide relationSideTo = RelationSide.FromGuidRelationEndTo(relation);
            this.dtoRelationAddition.AddRelationToDTO(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
            this.dbEntityMethodsAddition.AddPropertyToBackendFile(relationSideTo, LogicProjectGeneration.DtoFolder, FileName);
        }

        protected override void AddOneToOneRelationSideFrom(Relation1To1 relation)
        {
        }

        protected override void AddOneToOneRelationSideTo(Relation1To1 relation)
        {
            this.Add1ToNRelationSideTo(new Relation1ToN(relation));
        }
    }
}