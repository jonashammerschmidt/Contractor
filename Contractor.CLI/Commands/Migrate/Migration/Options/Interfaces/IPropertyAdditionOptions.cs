namespace Contractor.CLI.Migration
{
    public interface IPropertyAdditionOptions : IEntityAdditionOptions
    {
        PropertyTypes PropertyType { get; set; }

        string PropertyName { get; set; }

        string PropertyTypeExtra { get; set; }

        bool IsOptional { get; set; }
    }
}