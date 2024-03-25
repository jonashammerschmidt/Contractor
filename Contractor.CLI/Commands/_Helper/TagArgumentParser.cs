using CaseExtensions;
using Contractor.CLI.Tools;
using Contractor.Core.Generation;
using Contractor.Core.MetaModell;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.CLI.Commands._Helper
{
    public class TagArgumentParser
    {
        public static void AddTags(string[] args, GenerationOptions options)
        {
            if (ArgumentParser.HasArgument(args, "-t", "--tags"))
            {
                options.Tags = FromString(ArgumentParser.ExtractArgument(args, "-t", "--tags"));
            }
        }

        private static List<ClassGenerationTag> FromString(string data)
        {
            return data
                .Split(",")
                .Select(item => Parse(item))
                .Where(item => item.HasValue)
                .Select(item => item.Value)
                .SelectMany(item => GetChildren(item).Append(item))
                .ToList();
        }

        private static ClassGenerationTag? Parse(string text)
        {
            return text.ToCamelCase() switch
            {
                "backend" => ClassGenerationTag.BACKEND,
                "api" => ClassGenerationTag.BACKEND_API,
                "contract" => ClassGenerationTag.BACKEND_CONTRACT,
                "contractLogic" => ClassGenerationTag.BACKEND_CONTRACT_LOGIC,
                "contractPersistence" => ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE,
                "logic" => ClassGenerationTag.BACKEND_LOGIC,
                "logicTest" => ClassGenerationTag.BACKEND_LOGIC_TESTS,
                "persistence" => ClassGenerationTag.BACKEND_PERSISTENCE,
                "persistenceRepository" => ClassGenerationTag.BACKEND_PERSISTENCE_REPOSITORY,
                "persistenceDbContext" => ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT,
                "misc" => ClassGenerationTag.BACKEND_MISC,
                "frontend" => ClassGenerationTag.FRONTEND,
                "frontendModel" => ClassGenerationTag.FRONTEND_MODEL,
                "frontendPages" => ClassGenerationTag.FRONTEND_PAGES,
                _ => null,
            };
        }

        private static ClassGenerationTag[] GetChildren(ClassGenerationTag tag)
        {
            return tag switch
            {
                ClassGenerationTag.BACKEND => new[] {
                    ClassGenerationTag.BACKEND_API,
                    ClassGenerationTag.BACKEND_CONTRACT,
                    ClassGenerationTag.BACKEND_CONTRACT_LOGIC,
                    ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE,
                    ClassGenerationTag.BACKEND_LOGIC,
                    ClassGenerationTag.BACKEND_LOGIC_TESTS,
                    ClassGenerationTag.BACKEND_PERSISTENCE,
                    ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT,
                    ClassGenerationTag.BACKEND_PERSISTENCE_REPOSITORY,
                    ClassGenerationTag.BACKEND_MISC,
                },
                ClassGenerationTag.BACKEND_CONTRACT => new[] {
                    ClassGenerationTag.BACKEND_CONTRACT_LOGIC,
                    ClassGenerationTag.BACKEND_CONTRACT_PERSISTENCE,
                },
                ClassGenerationTag.BACKEND_PERSISTENCE => new[] {
                    ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT,
                },
                ClassGenerationTag.FRONTEND => new[] { 
                    ClassGenerationTag.FRONTEND,
                    ClassGenerationTag.FRONTEND_MODEL,
                    ClassGenerationTag.FRONTEND_PAGES,
                },
                _ => new ClassGenerationTag[] { },
            };
        }
    }
}