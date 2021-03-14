namespace Contractor.Core.Options
{
    public class ContractorOptions : IContractorOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string FrontendDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string DbProjectName { get; set; }

        public ContractorOptions()
        {
        }

        public ContractorOptions(IContractorOptions options)
        {
            this.FrontendDestinationFolder = options.FrontendDestinationFolder;
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.DbProjectName = options.DbProjectName;
        }
    }
}