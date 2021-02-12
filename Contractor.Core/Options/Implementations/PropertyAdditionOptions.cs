using Contractor.Core.Helpers;

namespace Contractor.Core.Jobs
{
    public class PropertyAdditionOptions : EntityAdditionOptions, IPropertyAdditionOptions
    {
        public string PropertyType { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeExtra { get; set; }

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
        }

        public static bool Validate(IPropertyAdditionOptions options)
        {
            if (!EntityAdditionOptions.Validate(options) ||
               string.IsNullOrEmpty(options.PropertyName) ||
               string.IsNullOrEmpty(options.PropertyType) ||
               !options.PropertyName.IsAlpha())
            {
                return false;
            }

            options.PropertyName = options.PropertyName.UpperFirstChar();

            return true;
        }
    }
}