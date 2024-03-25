using Contractor.Core.MetaModell;
using System.IO;
using System.Linq;

namespace Contractor.Core.Tools
{
    public class PathService
    {
        // Backend

        public string GetAbsolutePathForBackend(GenerationOptions options, params string[] paths)
        {
            string absolutePath = Path.Combine(options.Paths.BackendDestinationFolder, paths.Aggregate(Path.Combine));
            absolutePath = ModellNameReplacements.ReplaceOptionsPlaceholders(options, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackend(Module module, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForBackend(module.Options, paths);
            absolutePath = ModellNameReplacements.ReplaceModulePlaceholders(module, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackend(Entity entity, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForBackend(entity.Module, paths);
            absolutePath = ModellNameReplacements.ReplaceEntityPlaceholders(entity, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackend(Property property, params string[] paths)
        {
            return GetAbsolutePathForBackend(property.Entity, paths);
        }

        public string GetAbsolutePathForBackend(Relation relation, params string[] paths)
        {
            return GetAbsolutePathForBackend(relation.EntityTo, paths);
        }

        // Backend.Generated

        public string GetAbsolutePathForBackendGenerated(GenerationOptions options, params string[] paths)
        {
            string absolutePath = Path.Combine(options.Paths.BackendGeneratedDestinationFolder, paths.Aggregate(Path.Combine));
            absolutePath = ModellNameReplacements.ReplaceOptionsPlaceholders(options, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackendGenerated(Module module, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForBackendGenerated(module.Options, paths);
            absolutePath = ModellNameReplacements.ReplaceModulePlaceholders(module, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackendGenerated(Entity entity, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForBackendGenerated(entity.Module, paths);
            absolutePath = ModellNameReplacements.ReplaceEntityPlaceholders(entity, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForBackendGenerated(Property property, params string[] paths)
        {
            return GetAbsolutePathForBackendGenerated(property.Entity, paths);
        }

        public string GetAbsolutePathForBackendGenerated(Relation relation, params string[] paths)
        {
            return GetAbsolutePathForBackendGenerated(relation.EntityTo, paths);
        }

        // Database

        public string GetAbsolutePathForDatabase(GenerationOptions options, params string[] paths)
        {
            string absolutePath = Path.Combine(options.Paths.DbDestinationFolder, paths.Aggregate(Path.Combine));
            absolutePath = ModellNameReplacements.ReplaceOptionsPlaceholders(options, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForDatabase(Module module, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForDatabase(module.Options, paths);
            absolutePath = ModellNameReplacements.ReplaceModulePlaceholders(module, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForDatabase(Entity entity, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForDatabase(entity.Module, paths);
            absolutePath = ModellNameReplacements.ReplaceEntityPlaceholders(entity, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForDatabase(Property property, params string[] paths)
        {
            return GetAbsolutePathForDatabase(property.Entity, paths);
        }

        public string GetAbsolutePathForDatabase(Relation relation, params string[] paths)
        {
            return GetAbsolutePathForDatabase(relation.EntityTo, paths);
        }

        // Frontend

        public string GetAbsolutePathForFrontend(GenerationOptions options, params string[] paths)
        {
            string absolutePath = Path.Combine(options.Paths.FrontendDestinationFolder, paths.Aggregate(Path.Combine));
            absolutePath = ModellNameReplacements.ReplaceOptionsPlaceholders(options, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForFrontend(Module module, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForFrontend(module.Options, paths);
            absolutePath = ModellNameReplacements.ReplaceModulePlaceholders(module, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForFrontend(Entity entity, params string[] paths)
        {
            string absolutePath = GetAbsolutePathForFrontend(entity.Module, paths);
            absolutePath = ModellNameReplacements.ReplaceEntityPlaceholders(entity, absolutePath);
            return absolutePath;
        }

        public string GetAbsolutePathForFrontend(Property property, params string[] paths)
        {
            return GetAbsolutePathForFrontend(property.Entity, paths);
        }

        public string GetAbsolutePathForFrontend(Relation relation, params string[] paths)
        {
            return GetAbsolutePathForFrontend(relation.EntityTo, paths);
        }
    }
}