using Contractor.Core.Helpers;

namespace Contractor.Core.Jobs
{
    public class RelationAdditionOptions : ContractorOptions, IRelationAdditionOptions
    {
        public string DomainFrom { get; set; }

        public string EntityNameFrom { get; set; }

        public string EntityNamePluralFrom { get; set; }

        public string DomainTo { get; set; }

        public string EntityNameTo { get; set; }

        public string EntityNamePluralTo { get; set; }

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

        public RelationAdditionOptions()
        {
        }

        public RelationAdditionOptions(IContractorOptions options)
        {
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

        public static IPropertyAdditionOptions GetPropertyForFrom(IRelationAdditionOptions options, string propertyType, string propertyName)
        {
            return new PropertyAdditionOptions(options)
            {
                Domain = options.DomainFrom,
                EntityName = options.EntityNameFrom,
                EntityNamePlural = options.EntityNamePluralFrom,
                PropertyType = propertyType,
                PropertyName = propertyName
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

        public static IPropertyAdditionOptions GetPropertyForTo(IRelationAdditionOptions options, string propertyType, string propertyName)
        {
            return new PropertyAdditionOptions(options)
            {
                Domain = options.DomainTo,
                EntityName = options.EntityNameTo,
                EntityNamePlural = options.EntityNamePluralTo,
                PropertyType = propertyType,
                PropertyName = propertyName
            };
        }

        public static bool Validate(IRelationAdditionOptions options)
        {
            if (string.IsNullOrEmpty(options.DomainFrom) ||
               string.IsNullOrEmpty(options.EntityNameFrom) ||
               string.IsNullOrEmpty(options.EntityNamePluralFrom) ||
               string.IsNullOrEmpty(options.DomainTo) ||
               string.IsNullOrEmpty(options.EntityNameTo) ||
               string.IsNullOrEmpty(options.EntityNamePluralTo) ||
               !options.DomainFrom.IsAlpha() ||
               !options.EntityNameFrom.IsAlpha() ||
               !options.EntityNamePluralFrom.IsAlpha() ||
               !options.DomainTo.IsAlpha() ||
               !options.EntityNameTo.IsAlpha() ||
               !options.EntityNamePluralTo.IsAlpha())
            {
                return false;
            }

            options.DomainFrom = options.DomainFrom.UpperFirstChar();
            options.EntityNameFrom = options.EntityNameFrom.UpperFirstChar();
            options.EntityNamePluralFrom = options.EntityNamePluralFrom.UpperFirstChar();
            options.DomainTo = options.DomainTo.UpperFirstChar();
            options.EntityNameTo = options.EntityNameTo.UpperFirstChar();
            options.EntityNamePluralTo = options.EntityNamePluralTo.UpperFirstChar();

            return true;
        }
    }
}