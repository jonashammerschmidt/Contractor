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

                case PropertyType.DateTime:
                    return
                        $"                entity.Property(e => e.{property.Name})\n" +
                        $"                    .IsRequired({(!property.IsOptional).ToString().ToLower()})\n" +
                        $"                    .HasColumnType(\"datetime\");";

                default:
                    return
                        $"                entity.Property(e => e.{property.Name})\n" +
                        $"                    .IsRequired({(!property.IsOptional).ToString().ToLower()});";
            }
        }
    }
}