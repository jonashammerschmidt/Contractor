using Contractor.Core.Helpers;

namespace Contractor.Core.Jobs
{
    public class EntityAdditionOptions : DomainAdditionOptions, IEntityAdditionOptions
    {
        public string EntityName { get; set; }

        public string EntityNamePlural { get; set; }

        public bool ForMandant { get; set; }

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
            this.ForMandant = options.ForMandant;
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