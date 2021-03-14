using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Database
{
    internal class DbTableGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(DBProjectGeneration.TemplateFolder, "EntitiesTableTemplate.txt");

        private static readonly string FileName = "Entities.sql";

        private readonly DbTableAddition dbTableAddition;
        private readonly DbTablePropertyAddition dbTablePropertyAddition;
        private readonly DbTableRelationContraintAddition dbTableRelationContraintAddition;

        public DbTableGeneration(
            DbTableAddition dbTableAddition,
            DbTablePropertyAddition dbTablePropertyAddition,
            DbTableRelationContraintAddition dbTableRelationContraintAddition)
        {
            this.dbTableAddition = dbTableAddition;
            this.dbTablePropertyAddition = dbTablePropertyAddition;
            this.dbTableRelationContraintAddition = dbTableRelationContraintAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            string dbTableTemplateFileName = TemplateFileName.GetFileNameForEntityAddition(options, TemplatePath);
            this.dbTableAddition.AddEntityCore(options, DBProjectGeneration.DomainFolder, dbTableTemplateFileName, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
            this.dbTablePropertyAddition.AddProperty(options, DBProjectGeneration.DomainFolder, FileName);
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
            // To
            IPropertyAdditionOptions optionsTo =
                RelationAdditionOptions.GetPropertyForTo(options, "Guid", $"{options.EntityNameFrom}Id");
            this.dbTablePropertyAddition.AddProperty(optionsTo, DBProjectGeneration.DomainFolder, FileName);
            this.dbTableRelationContraintAddition.AddContraint(options, DBProjectGeneration.DomainFolder, FileName);
        }
    }
}