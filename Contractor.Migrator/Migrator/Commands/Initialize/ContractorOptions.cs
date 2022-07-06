using Contractor.Core.Options;
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

        public Dictionary<string, string> Replacements { get; set; }

        public bool IsVerbose { get; set; }
    }
}