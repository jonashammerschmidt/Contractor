using Contractor.Core.Helpers;
using System;
using System.Text.RegularExpressions;

namespace Contractor.Core.Tools
{
    public class ModellNameReplacements
    {
        public static string ReplaceOptionsPlaceholders(ContractorGenerationOptions options, string text)
        {
            text = ReplaceOptionsPlaceholders(options, text);
            text = text.Replace("DbContextName", options.Paths.DbContextName);
            text = text.Replace("DbProjectName", options.Paths.DbProjectName);
            text = text.Replace("ProjectName", options.Paths.ProjectName);

            return text;
        }

        public static string ReplaceModulePlaceholders(Module module, string text)
        {
            text = ReplaceOptionsPlaceholders(module.Options, text);
            text = text.Replace("domain-kebab", module.NameKebab);
            text = text.Replace("Domain", module.Name);

            return text;
        }

        public static string ReplaceEntityPlaceholders(Entity entity, string text)
        {
            text = ReplaceGuidPlaceholders(text, entity.Name);
            text = ReplaceModulePlaceholders(entity.Module, text);
            text = text.Replace("RequestScopeDomain", entity.ScopeEntity.Module.Name);
            text = text.Replace("RequestScopes", entity.ScopeEntity.NamePlural);
            text = text.Replace("RequestScope", entity.ScopeEntity.Name);
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