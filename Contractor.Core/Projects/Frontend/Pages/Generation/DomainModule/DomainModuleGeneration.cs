﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class DomainModuleGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "domain-kebab-pages.module.template.txt");

        private static readonly string FileName = "domain-kebab-pages.module.ts";

        private readonly FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition;

        public DomainModuleGeneration(
            FrontendPagesDomainCoreAddition frontendPagesDomainCoreAddition)
        {
            this.frontendPagesDomainCoreAddition = frontendPagesDomainCoreAddition;
        }

        protected override void AddModuleActions(Module module)
        {
            this.frontendPagesDomainCoreAddition.AddEntityCore(module, PagesProjectGeneration.PagesFolder, TemplatePath, FileName);
        }

        protected override void AddEntity(Entity entity)
        {
        }

        protected override void AddProperty(IPropertyAdditionOptions options)
        {
        }

        protected override void Add1ToNRelation(IRelationAdditionOptions options)
        {
        }

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}