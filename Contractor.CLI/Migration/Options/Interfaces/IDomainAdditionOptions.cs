namespace Contractor.CLI.Migration
{
    public interface IDomainAdditionOptions : IContractorOptions
    {
        string Domain { get; set; }
    }
}