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
        private readonly UsingStatementAddition usingStatementAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicRelationAddition logicRelationAddition,
            UsingStatementAddition usingStatementAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicRelationAddition = logicRelationAddition;
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
            this.logicRelationAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName, usingStatement, false);
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            // To
            string usingStatement = $"{options.ProjectName}.Contract.Persistence.Modules.{options.DomainFrom}.{options.EntityNamePluralFrom}";
            this.logicRelationAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName, usingStatement, true);
        }
    }
}