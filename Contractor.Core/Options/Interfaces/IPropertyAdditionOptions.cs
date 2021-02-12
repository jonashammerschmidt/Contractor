namespace Contractor.Core.Jobs
{
    public interface IPropertyAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        string PropertyTypeExtra { get; set; }
    }
}