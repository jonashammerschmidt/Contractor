using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class DatabaseDbContextPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            switch (property.Type)
            {
                case PropertyTypes.String:
                    return 
                        $"                entity.Property(e => e.{property.Name})\n" +
                        $"                    .IsRequired({(!property.IsOptional).ToString().ToLower()})\n" +
                        $"                    .HasMaxLength({property.TypeExtra});";

                case PropertyTypes.DateTime:
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