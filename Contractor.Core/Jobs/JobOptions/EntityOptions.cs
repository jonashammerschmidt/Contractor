namespace Contractor.Core.Jobs
{
    public class EntityOptions : DomainOptions, IEntityOptions
    {
        public string EntityName { get; set; }

        public string EntityNamePlural { get; set; }

        public bool ForMandant { get; set; }

        public string EntityNameLower
        {
            get
            {
                return char.ToLower(EntityName[0]) + EntityName.Substring(1);
            }
        }

        public string EntityNamePluralLower
        {
            get
            {
                return char.ToLower(EntityNamePlural[0]) + EntityNamePlural.Substring(1);
            }
        }

        public EntityOptions()
        {
        }

        public EntityOptions(IContractorOptions options) : base(options)
        {
        }

        public EntityOptions(IDomainOptions options) : base(options)
        {
        }
    }
}