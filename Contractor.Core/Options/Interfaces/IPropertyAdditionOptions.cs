namespace Contractor.Core.Options
{
    public interface IPropertyAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        string PropertyTypeExtra { get; set; }
    }
}