namespace Contractor.Core.Jobs
{
    public interface IContractorOptions
    {
        string BackendDestinationFolder { get; set; }

        string DbDestinationFolder { get; set; }

        string ProjectName { get; set; }

        string DbProjectName { get; set; }
    }
}