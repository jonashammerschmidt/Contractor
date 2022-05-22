using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    public static class DatabaseDbContextPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return 
                        $"                entity.Property(e => e.{options.PropertyName})\n" +
                        $"                    .IsRequired({(!options.IsOptional).ToString().ToLower()})\n" +
                        $"                    .HasMaxLength({options.PropertyTypeExtra});";

                case PropertyTypes.DateTime:
                    return
                        $"                entity.Property(e => e.{options.PropertyName})\n" +
                        $"                    .IsRequired({(!options.IsOptional).ToString().ToLower()})\n" +
                        $"                    .HasColumnType(\"datetime\");";

                default:
                    return
                        $"                entity.Property(e => e.{options.PropertyName})\n" +
                        $"                    .IsRequired({(!options.IsOptional).ToString().ToLower()});";
            }
        }
    }
}