﻿using Contractor.Core.Options;
using Contractor.Core.Tools;
using System.IO;

namespace Contractor.Core.Projects.Frontend.Pages
{
    [ClassGenerationTags(new[] { ClassGenerationTag.FRONTEND, ClassGenerationTag.FRONTEND_PAGES })]
    internal class EntityUpdatePageScssGeneration : ClassGeneration
    {
        private static readonly string TemplatePath =
            Path.Combine(PagesProjectGeneration.TemplateFolder, "entity-kebab-update.page.scss.template.txt");

        private static readonly string FileName = "dialogs\\update\\entity-kebab-update.dialog.scss";

        private readonly FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition;

        public EntityUpdatePageScssGeneration(
            FrontendPagesEntityCoreAddition frontendPagesEntityCoreAddition)
        {
            this.frontendPagesEntityCoreAddition = frontendPagesEntityCoreAddition;
        }

        protected override void AddModuleActions(Module module)
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

        protected override void AddOneToOneRelation(IRelationAdditionOptions options)
        {
        }
    }
}