namespace Contractor.Core.Options
{
    public interface IPropertyAdditionOptions : IEntityAdditionOptions
    {
        PropertyTypes PropertyType { get; set; }

        string PropertyName { get; set; }

        string PropertyTypeExtra { get; set; }
    }
}