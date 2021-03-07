using Contractor.Core.Helpers;

namespace Contractor.Core.Options
{
    public class EntityAdditionOptions : DomainAdditionOptions, IEntityAdditionOptions
    {
        public string EntityName { get; set; }

        public string EntityNamePlural { get; set; }

        public bool HasRequestScope
        {
            get
            {
                return !string.IsNullOrEmpty(RequestScopeName) && !string.IsNullOrEmpty(RequestScopeNamePlural);
            }
        }

        public string RequestScopeName { get; set; }

        public string RequestScopeNamePlural { get; set; }

        public string RequestScopeNameLower
        {
            get
            {
                return char.ToLower(RequestScopeName[0]) + RequestScopeName.Substring(1);
            }
        }

        public string EntityNameLower
        {
            get
            {
                return char.ToLower(EntityName[0]) + EntityName.Substring(1);
            }
        }

        public string EntityNamePluralLower
        {
            get
            {
                return char.ToLower(EntityNamePlural[0]) + EntityNamePlural.Substring(1);
            }
        }

        public EntityAdditionOptions()
        {
        }

        public EntityAdditionOptions(IContractorOptions options) : base(options)
        {
        }

        public EntityAdditionOptions(IDomainAdditionOptions options) : base(options)
        {
        }

        public EntityAdditionOptions(IEntityAdditionOptions options) : base(options)
        {
            this.EntityName = options.EntityName;
            this.EntityNamePlural = options.EntityNamePlural;
            this.RequestScopeName = options.RequestScopeName;
            this.RequestScopeNamePlural = options.RequestScopeNamePlural;
        }

        public static bool Validate(IEntityAdditionOptions options)
        {
            if (!DomainAdditionOptions.Validate(options) ||
               string.IsNullOrEmpty(options.EntityName) ||
               string.IsNullOrEmpty(options.EntityNamePlural) ||
               !options.EntityName.IsAlpha() ||
               !options.EntityNamePlural.IsAlpha())
            {
                return false;
            }

            options.EntityName = options.EntityName.UpperFirstChar();
            options.EntityNamePlural = options.EntityNamePlural.UpperFirstChar();

            return true;
        }
    }
}