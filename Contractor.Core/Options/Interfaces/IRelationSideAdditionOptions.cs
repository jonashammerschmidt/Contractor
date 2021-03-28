namespace Contractor.Core.Options
{
    internal interface IRelationSideAdditionOptions : IEntityAdditionOptions
    {
        string PropertyType { get; set; }

        string PropertyName { get; set; }
    }
}