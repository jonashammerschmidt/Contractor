using System.Linq;
using Contractor.Core.MetaModell;

namespace Contractor.Core.BaseClasses
{
    public class PurposeDtoInterfaceCompatibilityChecker
    {
        public static bool IsInterfaceCompatible(PurposeDto purposeDto, Interface interfaceItem)
        {
            if (EntityInterfaceCompatibilityChecker.IsInterfaceCompatible(purposeDto.Entity, interfaceItem) == EntityInterfaceCompatibility.None)
            {
                return false;
            }

            interfaceItem = interfaceItem.ToFlatInterface();
            foreach (var relation in interfaceItem.Relations)
            {
                if (!purposeDto.Properties
                    .Any(property => property.PathItems.First().PropertyName == (relation.PropertyName ?? relation.TargetEntityName)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}