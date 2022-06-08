using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    [ClassGenerationTags(new[] { ClassGenerationTag.BACKEND, ClassGenerationTag.BACKEND_LOGIC })]
    internal class EntitiesCrudLogicGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntitiesCrudLogicTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogic.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudLogicRelationAddition logicRelationAddition;
        private readonly UniqueEntitiesCrudLogicRelationAddition uniquelogicRelationAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicRelationAddition logicRelationAddition,
            UniqueEntitiesCrudLogicRelationAddition uniquelogicRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicRelationAddition = logicRelationAddition;
            this.uniquelogicRelationAddition = uniquelogicRelationAddition;
        }

        protected override void AddModuleActions(Module module)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, LogicProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            string usingStatement = $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}";
            this.logicRelationAddition.Edit(options, LogicProjectGeneration.DomainFolder, FileName, usingStatement);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // To
            string usingStatement = $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}";
            this.uniquelogicRelationAddition.Edit(options, LogicProjectGeneration.DomainFolder, FileName, usingStatement);
        }
    }
}