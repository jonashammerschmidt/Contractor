namespace Contractor.Core.Jobs
{
    public interface IEntityOptions : IDomainOptions
    {
        string EntityName { get; set; }

        string EntityNamePlural { get; set; }

        bool ForMandant { get; set; }

        string EntityNameLower { get; }

        string EntityNamePluralLower { get; }
    }
}