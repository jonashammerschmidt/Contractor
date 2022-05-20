using Contractor.Core.Helpers;

namespace Contractor.Core.Options
{
    public class RelationAdditionOptions : ContractorOptions, IRelationAdditionOptions
    {
        private string domainFrom;
        private string entityNameFrom;
        private string entityNamePluralFrom;
        private string propertyNameFrom;

        private string domainTo;
        private string entityNameTo;
        private string entityNamePluralTo;
        private string propertyNameTo;

        public bool IsOptional { get; set; }

        public bool HasClusteredIndex { get; set; }

        public bool HasNonClusteredIndex { get; set; }

        public string DomainFrom
        {
            get { return domainFrom; }
            set { domainFrom = value.ToVariableName(); }
        }

        public string EntityNameFrom
        {
            get { return entityNameFrom; }
            set { entityNameFrom = value.ToVariableName(); }
        }

        public string EntityNamePluralFrom
        {
            get { return entityNamePluralFrom; }
            set { entityNamePluralFrom = value.ToVariableName(); }
        }
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

        public string PropertyNameFrom
        {
            get { return propertyNameFrom; }
            set { propertyNameFrom = value.ToVariableName(); }
        }

        public string DomainTo
        {
            get { return domainTo; }
            set { domainTo = value.ToVariableName(); }
        }

        public string EntityNameTo
        {
            get { return entityNameTo; }
            set { entityNameTo = value.ToVariableName(); }
        }

        public string EntityNamePluralTo
        {
            get { return entityNamePluralTo; }
            set { entityNamePluralTo = value.ToVariableName(); }
        }

        public string PropertyNameTo
        {
            get { return propertyNameTo; }
            set { propertyNameTo = value.ToVariableName(); }
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

        public RelationAdditionOptions(IContractorOptions options) : base(options)
        {
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

        public static IEntityAdditionOptions GetPropertyForTo(IRelationAdditionOptions options)
        {
            return new EntityAdditionOptions(options)
            {
                Domain = options.DomainTo,
                EntityName = options.EntityNameTo,
                EntityNamePlural = options.EntityNamePluralTo,
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
                IsOptional = options.IsOptional,
                HasClusteredIndex = options.HasClusteredIndex,
                HasNonClusteredIndex = options.HasNonClusteredIndex,
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
                PropertyName = options.PropertyNameFrom + (propertyType.Equals("Guid") ? "Id" : ""),
                HasClusteredIndex = options.HasClusteredIndex,
                HasNonClusteredIndex = options.HasNonClusteredIndex,
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
               !options.PropertyNameTo.IsAlpha() ||
               (options.HasClusteredIndex && options.HasNonClusteredIndex))
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