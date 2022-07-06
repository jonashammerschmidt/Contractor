namespace Contractor.Core.Options
{
    public interface IEntityAdditionOptions : IDomainAdditionOptions
    {
        string EntityName { get; set; }

        string EntityNamePlural { get; set; }

        bool HasRequestScope { get; }

        string RequestScopeDomain { get; set; }

        string RequestScopeName { get; set; }

        string RequestScopeNamePlural { get; set; }

        string RequestScopeNameLower { get; }

        string EntityNameLower { get; }

        string EntityNamePluralLower { get; }
    }
}