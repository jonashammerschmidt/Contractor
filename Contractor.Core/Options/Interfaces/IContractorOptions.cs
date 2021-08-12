namespace Contractor.Core.Options
{
    public interface IContractorOptions
    {
        string BackendDestinationFolder { get; set; }

        string DbDestinationFolder { get; set; }

        string FrontendDestinationFolder { get; set; }

        string ProjectName { get; set; }

        string DbProjectName { get; set; }

        bool IsVerbose { get; set; }
    }
}