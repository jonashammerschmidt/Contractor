using Contractor.Core.Helpers;

namespace Contractor.Core.MetaModell
{
    public class InterfaceRelation
    {
        private string targetEntityName;
        private string propertyName;

        public string TargetEntityName
        {
            get => targetEntityName;
            set => targetEntityName = value.ToVariableName();
        }

        public string PropertyName
        {
            get => propertyName;
            set => propertyName = value?.ToVariableName();
        }
    }
}