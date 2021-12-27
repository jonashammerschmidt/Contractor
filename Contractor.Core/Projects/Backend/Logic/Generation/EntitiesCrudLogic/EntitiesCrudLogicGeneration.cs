using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Backend.Logic
{
    internal class EntitiesCrudLogicGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntitiesCrudLogicTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogic.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudLogicRelationAddition logicRelationAddition;
        private readonly UniqueEntitiesCrudLogicRelationAddition uniquelogicRelationAddition;
        private readonly UsingStatementAddition usingStatementAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicRelationAddition logicRelationAddition,
            UniqueEntitiesCrudLogicRelationAddition uniquelogicRelationAddition,
            UsingStatementAddition usingStatementAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicRelationAddition = logicRelationAddition;
            this.uniquelogicRelationAddition = uniquelogicRelationAddition;
            this.usingStatementAddition = usingStatementAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.entityCoreAddition.AddEntityCore(options, LogicProjectGeneration.DomainFolder, TemplatePath, FileName);
            this.usingStatementAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName, "Microsoft.EntityFrameworkCore");
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