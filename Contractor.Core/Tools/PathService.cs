using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;
using System.IO;

namespace Contractor.Core.Tools
{
    internal class PathService
    {
        public string GetAbsolutePathForBackend(ContractorGenerationOptions options, string domainFolder)
        {
            return Path.Combine(options.Paths.BackendDestinationFolder, domainFolder);
        }

        public string GetAbsolutePathForBackend(Entity entity, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForBackend(entity.Module.Options, domainFolder);
            absolutePath = absolutePath.Replace("Domain", entity.Module.Name);
            absolutePath = absolutePath.Replace("Entities", entity.NamePlural);

            return absolutePath;
        }

        internal string GetAbsolutePathForDatabase(IRelationAdditionOptions options, string v)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForDatabase(IPropertyAdditionOptions options, string v)
        {
            throw new NotImplementedException();
        }

        public string GetAbsolutePathForDatabase(ContractorGenerationOptions options, string domainFolder)
        {
            return Path.Combine(options.Paths.DbDestinationFolder, domainFolder);
        }

        public string GetAbsolutePathForDatabase(Entity entity, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForDatabase(entity.Module.Options, domainFolder);
            absolutePath = absolutePath.Replace("Domain", entity.Module.Name);
            absolutePath = absolutePath.Replace("Entities", entity.NamePlural);
            return absolutePath;
        }

        internal string GetAbsolutePathForFrontend(IEntityAdditionOptions entityOptions, string domainFolder)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForBackend(IPropertyAdditionOptions options, string domainFolder)
        {
            throw new NotImplementedException();
        }

        public string GetAbsolutePathForFrontend(Module module, string domainFolder)
        {
            string absolutePath = Path.Combine(module.Options.Paths.FrontendDestinationFolder, domainFolder);
            absolutePath = absolutePath.Replace("domain-kebab", StringConverter.PascalToKebabCase(module.Name));
            return absolutePath;
        }

        internal string GetAbsolutePathForBackend(IEntityAdditionOptions entityOptions, string domainFolder)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForDatabase(IEntityAdditionOptions entityOptions, string domainFolder)
        {
            throw new NotImplementedException();
        }

        public string GetAbsolutePathForFrontend(Entity entity, string domainFolder)
        {
            string absolutePath = GetAbsolutePathForFrontend(entity.Module, domainFolder);
            absolutePath = absolutePath.Replace("entity-kebab", StringConverter.PascalToKebabCase(entity.Name));
            absolutePath = absolutePath.Replace("entities-kebab", StringConverter.PascalToKebabCase(entity.NamePlural));
            return absolutePath;
        }

        internal string GetAbsolutePathForDatabase(IRelationSideAdditionOptions options, string domainFolder)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForBackend(IRelationSideAdditionOptions options, string domainFolder)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForFrontend(IPropertyAdditionOptions options, string domainFolder)
        {
            throw new NotImplementedException();
        }

        internal string GetAbsolutePathForFrontend(IRelationSideAdditionOptions options, string domainFolder)
        {
            throw new NotImplementedException();
        }
    }
}