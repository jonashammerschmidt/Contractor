namespace Contractor.Core.Jobs
{
    public interface IPropertyOptions : IEntityOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        string PropertyTypeExtra { get; set; }
    }
}