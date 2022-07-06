namespace Contractor.Core.Options
{
    public interface IDomainAdditionOptions : IContractorOptions
    {
        string Domain { get; set; }
    }
}