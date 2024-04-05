using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public class InterfaceRelation
    {
        private string entityNameFrom;
        private string propertyNameFrom;

        public string EntityNameFrom
        {
            get => entityNameFrom;
            set => entityNameFrom = value.ToVariableName();
        }

        public string PropertyNameFrom
        {
            get => propertyNameFrom;
            set => propertyNameFrom = value?.ToVariableName();
        }
    }
}