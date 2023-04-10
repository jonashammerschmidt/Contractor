using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    public class ModellNameReplacements
    {
        public static string ReplaceOptionsPlaceholders(ContractorGenerationOptions options, string text)
        {
            text = text.Replace("DbContextName", options.Paths.DbContextName);
            text = text.Replace("DbProjectName", options.Paths.DbProjectName);
            text = text.Replace("GeneratedProjectName", options.Paths.GeneratedProjectName);
            text = text.Replace("ProjectName", options.Paths.ProjectName);

            return text;
        }

        public static string ReplaceModulePlaceholders(Module module, string text)
        {
            text = text.Replace("domain-kebab", module.NameKebab);
            text = text.Replace("Domain", module.Name);

            return text;
        }

        public static string ReplaceEntityPlaceholders(Entity entity, string text)
        {
            text = ReplaceGuidPlaceholders(text, entity.Name);
            text = text.Replace("EntityFramework", "##EfCore##");

            if (entity.HasScope)
            {
                text = text.Replace("RequestScopeDomain", entity.ScopeEntity.Module.Name);
                text = text.Replace("RequestScopes", entity.ScopeEntity.NamePlural);
                text = text.Replace("requestScopes", entity.ScopeEntity.NamePluralLower);
                text = text.Replace("RequestScope", entity.ScopeEntity.Name);
                text = text.Replace("requestScope", entity.ScopeEntity.NameLower);
            }

            text = text.Replace("DisplayPropertyFallback", entity.DisplayPropertyFallback);
            text = text.Replace("displayPropertyFallback", entity.DisplayPropertyFallback.LowerFirstChar());
            text = text.Replace("DisplayProperty", entity.DisplayProperty.Name);
            text = text.Replace("displayProperty", entity.DisplayProperty.NameLower);
            text = text.Replace("entities-kebab", entity.NamePluralKebab);
            text = text.Replace("entity-kebab", entity.NameKebab);
            text = text.Replace("EntitiesReadable", entity.NamePluralReadable);
            text = text.Replace("EntityReadable", entity.NameReadable);
            text = text.Replace("Entities", entity.NamePlural);
            text = text.Replace("Entity", entity.Name);
            text = text.Replace("entities", entity.NamePluralLower);
            text = text.Replace("entity", entity.NameLower);
            text = text.Replace("##EfCore##", "EntityFramework");

            return text;
        }

        public static string ReplaceModulePlaceholdersCascading(Module module, string text)
        {
            text = ReplaceModulePlaceholders(module, text);
            text = ReplaceOptionsPlaceholders(module.Options, text);

            return text;
        }

        public static string ReplaceEntityPlaceholdersCascading(Entity entity, string text)
        {
            text = ReplaceEntityPlaceholders(entity, text);
            text = ReplaceModulePlaceholdersCascading(entity.Module, text);
            text = ReplaceGuidPlaceholders(text, entity.Name);

            return text;
        }

        private static string ReplaceGuidPlaceholders(string text, string seed)
        {
            Random random = new Random(IntHash.ComputeIntHash($"{seed}"));
            var regex = new Regex(Regex.Escape("{random-guid}"));
            int placeholderCount = text.Split(new[] { "{random-guid}" }, StringSplitOptions.None).Length - 1;

            for (int i = 0; i < placeholderCount; i++)
            {
                var guid = new byte[16];
                random.NextBytes(guid);
                text = regex.Replace(text, new Guid(guid).ToString(), 1);
            }

            return text;
        }
    }
}