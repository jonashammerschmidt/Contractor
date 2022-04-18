using CaseExtensions;
using Contractor.CLI.Tools;
using Contractor.Core.Options;
using Contractor.Core.Projects;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.CLI.Commands._Helper
{
    internal class TagArgumentParser
    {
        public static void AddTags(string[] args, IContractorOptions options)
        {
            if (ArgumentParser.HasArgument(args, "-t", "--tags"))
            {
                options.Tags = FromString(ArgumentParser.ExtractArgument(args, "-t", "--tags"));
            }
        }

        private static IEnumerable<ClassGenerationTag> FromString(string data)
        {
            return data
                .Split(",")
                .Select(item => Parse(item))
                .Where(item => item.HasValue)
                .Select(item => item.Value);
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
                "ef" => ClassGenerationTag.BACKEND_EF,
                "logic" => ClassGenerationTag.BACKEND_LOGIC,
                "logicTest" => ClassGenerationTag.BACKEND_LOGIC_TESTS,
                "persistence" => ClassGenerationTag.BACKEND_PERSISTENCE,
                "persistenceTests" => ClassGenerationTag.BACKEND_PERSISTENCE_TESTS,
                "database" => ClassGenerationTag.DATABASE,
                "frontend" => ClassGenerationTag.FRONTEND,
                "frontendModel" => ClassGenerationTag.FRONTEND_MODEL,
                "frontendPages" => ClassGenerationTag.FRONTEND_PAGES,
                _ => null,
            };
        }
    }
}