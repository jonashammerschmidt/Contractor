namespace Contractor.Core.Options
{
    public interface IEntityAdditionOptions : IDomainAdditionOptions
    {
        string EntityName { get; set; }

        string EntityNamePlural { get; set; }

        bool ForMandant { get; set; }

        string EntityNameLower { get; }

        string EntityNamePluralLower { get; }
    }
}