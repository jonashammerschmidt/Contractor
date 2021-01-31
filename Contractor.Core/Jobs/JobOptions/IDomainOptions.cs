namespace Contractor.Core.Jobs
{
    public interface IDomainOptions : IContractorOptions
    {
        string Domain { get; set; }
    }
}