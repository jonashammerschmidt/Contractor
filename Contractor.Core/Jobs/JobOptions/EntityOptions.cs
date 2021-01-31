namespace Contractor.Core.Jobs.EntityAddition
{
    public class EntityOptions : IDomainOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string DbProjectName { get; set; }

        public string Domain { get; set; }

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

        public EntityOptions(IContractorOptions options)
        {
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.DbProjectName = options.DbProjectName;
        }

        public EntityOptions(IDomainOptions options)
        {
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.DbProjectName = options.DbProjectName;
            this.Domain = options.Domain;
        }
    }
}