namespace Contractor.Core.Options
{
    public class RelationSideAdditionOptions : EntityAdditionOptions, IRelationSideAdditionOptions
    {
        public string PropertyType { get; set; }

        public string PropertyName { get; set; }

        public RelationSideAdditionOptions()
        {
        }

        public RelationSideAdditionOptions(IContractorOptions options) : base(options)
        {
        }
    }
}