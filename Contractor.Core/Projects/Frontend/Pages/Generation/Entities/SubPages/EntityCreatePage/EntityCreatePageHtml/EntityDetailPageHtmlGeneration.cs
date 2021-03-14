﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    internal class EntityCreatePageHtmlGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entities-create-detail.page.html.template.txt");

        private static readonly string FileName = "entities-kebab-create\\entities-kebab-create.page.html";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;

        public EntityCreatePageHtmlGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
        }

        protected override void AddDomain(IDomainAdditionOptions options)
        {
        }

        protected override void AddEntity(IEntityAdditionOptions options)
        {
            this.frontendPagesEntityCoreAddition.AddEntityCore(options, PagesProjectGeneration.DomainFolder, TemplatePath, FileName);
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }
    }
}