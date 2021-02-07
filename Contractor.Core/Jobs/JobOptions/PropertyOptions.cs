namespace Contractor.Core.Jobs
{
    public class PropertyOptions : EntityOptions, IPropertyOptions
    {
        public string PropertyType { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeExtra { get; set; }

        public PropertyOptions()
        {
        }

        public PropertyOptions(IContractorOptions options) : base(options)
        {
        }

        public PropertyOptions(IDomainOptions options) : base(options)
        {
        }
    }
}