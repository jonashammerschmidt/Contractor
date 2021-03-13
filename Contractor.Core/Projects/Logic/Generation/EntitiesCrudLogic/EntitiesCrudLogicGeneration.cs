using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Logic
{
    internal class EntitiesCrudLogicGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(LogicProjectGeneration.TemplateFolder, "EntitiesCrudLogicTemplate.txt");

        private static readonly string FileName = "EntitiesCrudLogic.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly EntitiesCrudLogicRelationAddition logicRelationAddition;

        public EntitiesCrudLogicGeneration(
            EntityCoreAddition entityCoreAddition,
            EntitiesCrudLogicRelationAddition logicRelationAddition)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.logicRelationAddition = logicRelationAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
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
            this.logicRelationAddition.Add(options, LogicProjectGeneration.DomainFolder, FileName, usingStatement);
        }
    }
}