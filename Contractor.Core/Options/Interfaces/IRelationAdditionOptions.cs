namespace Contractor.Core.Jobs
{
    public interface IRelationAdditionOptions : IContractorOptions
    {
        string DomainFrom { get; set; }

        string EntityNameFrom { get; set; }

        string EntityNameLowerFrom { get; }

        string EntityNamePluralFrom { get; set; }

        string EntityNamePluralLowerFrom { get; }

        string DomainTo { get; set; }

        string EntityNameLowerTo { get; }

        string EntityNameTo { get; set; }

        string EntityNamePluralLowerTo { get; }

        string EntityNamePluralTo { get; set; }
    }
}