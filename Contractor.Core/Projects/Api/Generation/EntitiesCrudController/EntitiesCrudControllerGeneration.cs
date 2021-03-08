using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Api
{
    internal class EntitiesCrudControllerGeneration : ClassGeneration
    {
        private static readonly string TemplatePath = 
            Path.Combine(ApiProjectGeneration.TemplateFolder, "EntitiesCrudControllerTemplate.txt");

        private static readonly string FileName = "EntitiesCrudController.cs";

        private readonly EntityCoreAddition entityCoreAddition;
        private readonly PathService pathService;

        public EntitiesCrudControllerGeneration(
            EntityCoreAddition entityCoreAddition,
            PathService pathService)
        {
            this.entityCoreAddition = entityCoreAddition;
            this.pathService = pathService;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.pathService.AddEntityFolder(options, ApiProjectGeneration.DomainFolder);
            this.entityCoreAddition.AddEntityCore(options, ApiProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}