namespace Contractor.Core.Options
{
    internal interface IRelationSideAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }

        bool IsOptional { get; set; }

        bool HasClusteredIndex { get; set; }

        bool HasNonClusteredIndex { get; set; }

        bool IsUnique { get; set; }
    }
}