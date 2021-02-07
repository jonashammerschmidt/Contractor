namespace Contractor.Core.Jobs
{
    public class DomainOptions : IDomainOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string DbProjectName { get; set; }

        public string Domain { get; set; }

        public DomainOptions()
        {
        }

        public DomainOptions(IContractorOptions options)
        {
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.DbProjectName = options.DbProjectName;
        }

        public DomainOptions(IDomainOptions options)
        {
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.DbProjectName = options.DbProjectName;
            this.Domain = options.Domain;
        }
    }
}