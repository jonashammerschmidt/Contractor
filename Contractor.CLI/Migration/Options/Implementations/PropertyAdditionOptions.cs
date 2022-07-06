using Contractor.Core.Helpers;

namespace Contractor.CLI.Migration
{
    public class PropertyAdditionOptions : EntityAdditionOptions, IPropertyAdditionOptions
    {
        private string propertyName;

        public PropertyTypes PropertyType { get; set; }

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value.ToVariableName(); }
        }

        public string PropertyTypeExtra { get; set; }

        public bool IsOptional { get; set; } = false;

        public PropertyAdditionOptions()
        {
        }

        public PropertyAdditionOptions(IContractorOptions options) : base(options)
        {
        }

        public PropertyAdditionOptions(IDomainAdditionOptions options) : base(options)
        {
        }

        public PropertyAdditionOptions(IEntityAdditionOptions options) : base(options)
        {
        }

        public PropertyAdditionOptions(IPropertyAdditionOptions options) : base(options)
        {
            this.PropertyType = options.PropertyType;
            this.PropertyName = options.PropertyName;
            this.PropertyTypeExtra = options.PropertyTypeExtra;
            this.IsOptional = options.IsOptional;
        }

        internal PropertyAdditionOptions(IRelationSideAdditionOptions options) : base(options)
        {
            this.PropertyName = options.PropertyName;
            this.IsOptional = options.IsOptional;
            switch (options.PropertyType)
            {
                case "Guid":
                    this.PropertyType = PropertyTypes.Guid;
                    break;
            };
        }

        public static bool Validate(IPropertyAdditionOptions options)
        {
            if (!EntityAdditionOptions.Validate(options) ||
               string.IsNullOrEmpty(options.PropertyName) ||
               !options.PropertyName.IsAlpha())
            {
                return false;
            }

            options.PropertyName = options.PropertyName.UpperFirstChar();

            return true;
        }
    }
}