using Contractor.Core.Helpers;

namespace Contractor.CLI.Migration
{
    public class EntityAdditionOptions : DomainAdditionOptions, IEntityAdditionOptions
    {

        private string entityName;

        private string entityNamePlural;
        
        private string requestScopeDomain;

        private string requestScopeName;

        private string requestScopeNamePlural;

        public string EntityName
        {
            get { return entityName; }
            set { entityName = value.ToVariableName(); }
        }

        public string EntityNameLower
        {
            get
            {
                return char.ToLower(EntityName[0]) + EntityName.Substring(1);
            }
        }

        public string EntityNamePlural
        {
            get { return entityNamePlural; }
            set { entityNamePlural = value.ToVariableName(); }
        }

        public string EntityNamePluralLower
        {
            get
            {
                return char.ToLower(EntityNamePlural[0]) + EntityNamePlural.Substring(1);
            }
        }

        public bool HasRequestScope
        {
            get
            {
                return !string.IsNullOrEmpty(RequestScopeName) && !string.IsNullOrEmpty(RequestScopeNamePlural);
            }
        }

        public string RequestScopeDomain
        {
            get { return requestScopeDomain; }
            set { requestScopeDomain = value?.ToVariableName(); }
        }

        public string RequestScopeName
        {
            get { return requestScopeName; }
            set { requestScopeName = value?.ToVariableName(); }
        }

        public string RequestScopeNameLower
        {
            get
            {
                return char.ToLower(RequestScopeName[0]) + RequestScopeName.Substring(1);
            }
        }

        public string RequestScopeNamePlural
        {
            get { return requestScopeNamePlural; }
            set { requestScopeNamePlural = value?.ToVariableName(); }
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
            this.RequestScopeDomain = options.RequestScopeDomain;
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