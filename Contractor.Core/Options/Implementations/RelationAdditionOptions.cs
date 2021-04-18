using Contractor.Core.Helpers;

namespace Contractor.Core.Options
{
    public class RelationAdditionOptions : ContractorOptions, IRelationAdditionOptions
    {
        public string DomainFrom { get; set; }

        public string EntityNameFrom { get; set; }

        public string EntityNamePluralFrom { get; set; }

        public string EntityNameLowerFrom
        {
            get
            {
                return char.ToLower(EntityNameFrom[0]) + EntityNameFrom.Substring(1);
            }
        }

        public string EntityNamePluralLowerFrom
        {
            get
            {
                return char.ToLower(EntityNamePluralFrom[0]) + EntityNamePluralFrom.Substring(1);
            }
        }

        public string PropertyNameFrom { get; set; }

        public string DomainTo { get; set; }

        public string EntityNameTo { get; set; }

        public string EntityNamePluralTo { get; set; }

        public string EntityNameLowerTo
        {
            get
            {
                return char.ToLower(EntityNameTo[0]) + EntityNameTo.Substring(1);
            }
        }

        public string EntityNamePluralLowerTo
        {
            get
            {
                return char.ToLower(EntityNamePluralTo[0]) + EntityNamePluralTo.Substring(1);
            }
        }

        public string PropertyNameTo { get; set; }

        public RelationAdditionOptions()
        {
        }

        public RelationAdditionOptions(IContractorOptions options)
        {
            this.FrontendDestinationFolder = options.FrontendDestinationFolder;
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.DbProjectName = options.DbProjectName;
            this.ProjectName = options.ProjectName;
        }

        public static IEntityAdditionOptions GetPropertyForFrom(IRelationAdditionOptions options)
        {
            return new EntityAdditionOptions(options)
            {
                Domain = options.DomainFrom,
                EntityName = options.EntityNameFrom,
                EntityNamePlural = options.EntityNamePluralFrom
            };
        }

        internal static IRelationSideAdditionOptions GetPropertyForFrom(IRelationAdditionOptions options, string propertyType)
        {
            return new RelationSideAdditionOptions(options)
            {
                Domain = options.DomainFrom,
                EntityName = options.EntityNameFrom,
                EntityNamePlural = options.EntityNamePluralFrom,
                PropertyType = propertyType,
                PropertyName = options.PropertyNameTo,
            };
        }

        public static IEntityAdditionOptions GetPropertyForTo(IRelationAdditionOptions options)
        {
            return new EntityAdditionOptions(options)
            {
                Domain = options.DomainTo,
                EntityName = options.EntityNameTo,
                EntityNamePlural = options.EntityNamePluralTo,
            };
        }

        internal static IRelationSideAdditionOptions GetPropertyForTo(IRelationAdditionOptions options, string propertyType)
        {
            return new RelationSideAdditionOptions(options)
            {
                Domain = options.DomainTo,
                EntityName = options.EntityNameTo,
                EntityNamePlural = options.EntityNamePluralTo,
                PropertyType = propertyType,
                PropertyName = options.PropertyNameFrom + (propertyType.Equals("Guid") ? "Id" : "")
            };
        }

        public static bool Validate(IRelationAdditionOptions options)
        {
            if (string.IsNullOrEmpty(options.DomainFrom) ||
               string.IsNullOrEmpty(options.EntityNameFrom) ||
               string.IsNullOrEmpty(options.EntityNamePluralFrom) ||
               string.IsNullOrEmpty(options.PropertyNameFrom) ||
               string.IsNullOrEmpty(options.DomainTo) ||
               string.IsNullOrEmpty(options.EntityNameTo) ||
               string.IsNullOrEmpty(options.EntityNamePluralTo) ||
               string.IsNullOrEmpty(options.PropertyNameTo) ||
               !options.DomainFrom.IsAlpha() ||
               !options.EntityNameFrom.IsAlpha() ||
               !options.EntityNamePluralFrom.IsAlpha() ||
               !options.PropertyNameFrom.IsAlpha() ||
               !options.DomainTo.IsAlpha() ||
               !options.EntityNameTo.IsAlpha() ||
               !options.EntityNamePluralTo.IsAlpha() ||
               !options.PropertyNameTo.IsAlpha())
            {
                return false;
            }

            options.DomainFrom = options.DomainFrom.UpperFirstChar();
            options.EntityNameFrom = options.EntityNameFrom.UpperFirstChar();
            options.EntityNamePluralFrom = options.EntityNamePluralFrom.UpperFirstChar();
            options.PropertyNameFrom = options.PropertyNameFrom.UpperFirstChar();
            options.DomainTo = options.DomainTo.UpperFirstChar();
            options.EntityNameTo = options.EntityNameTo.UpperFirstChar();
            options.EntityNamePluralTo = options.EntityNamePluralTo.UpperFirstChar();
            options.PropertyNameTo = options.PropertyNameTo.UpperFirstChar();

            return true;
        }
    }
}