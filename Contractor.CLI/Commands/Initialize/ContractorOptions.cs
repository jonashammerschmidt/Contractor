using Contractor.Core.Options;
using Contractor.Core.Projects;
using System.Collections.Generic;

namespace Contractor.CLI
{
    internal class ContractorOptions : IContractorOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string FrontendDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string DbProjectName { get; set; }

        public string DbContextName { get; set; }

        public Dictionary<string, string> Replacements { get; set; }

        public bool IsVerbose { get; set; }

        public IEnumerable<ClassGenerationTag> Tags { get; set; } = new List<ClassGenerationTag>();
    }
}