namespace Contractor.CLI.Migration
{
    public interface IRelationSideAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        bool IsOptional { get; set; }
    }
}