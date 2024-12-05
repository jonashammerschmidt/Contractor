using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    public static class DatabaseDbContextPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            switch (property.Type)
            {
                case PropertyType.String:
                    return
                        $"                entity.Property(e => e.{property.Name})\n" +
                        $"                    .IsRequired({(!property.IsOptional).ToString().ToLower()})\n" +
                        $"                    .HasMaxLength({property.TypeExtra});";

                default:
                    return
                        $"                entity.Property(e => e.{property.Name})\n" +
                        $"                    .IsRequired({(!property.IsOptional).ToString().ToLower()});";
            }
        }
    }
}