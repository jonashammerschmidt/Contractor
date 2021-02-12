using Contractor.Core.Jobs;

namespace Contractor.CLI
{
    public class ContractorOptions : IContractorOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string DbProjectName { get; set; }
    }
}