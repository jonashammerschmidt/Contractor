namespace Contractor.CLI.Migration
{
    internal interface IRelationSideAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        bool IsOptional { get; set; }
    }
}