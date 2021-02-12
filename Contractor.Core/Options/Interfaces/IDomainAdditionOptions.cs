namespace Contractor.Core.Jobs
{
    public interface IDomainAdditionOptions : IContractorOptions
    {
        string Domain { get; set; }
    }
}