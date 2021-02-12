using Contractor.Core.Helpers;

namespace Contractor.Core.Options
{
    public class DomainAdditionOptions : ContractorOptions, IDomainAdditionOptions
    {
        public string Domain { get; set; }

        public DomainAdditionOptions()
        {
        }

        public DomainAdditionOptions(IContractorOptions options) : base(options)
        {
        }

        public DomainAdditionOptions(IDomainAdditionOptions options) : base(options)
        {
            this.Domain = options.Domain;
        }

        public static bool Validate(IDomainAdditionOptions options)
        {
            if (string.IsNullOrEmpty(options.Domain) ||
               !options.Domain.IsAlpha())
            {
                return false;
            }

            options.Domain = options.Domain.UpperFirstChar();

            return true;
        }
    }
}