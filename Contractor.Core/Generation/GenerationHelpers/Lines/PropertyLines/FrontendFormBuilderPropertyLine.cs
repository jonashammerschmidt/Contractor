using Contractor.Core.Helpers;
using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendFormBuilderPropertyLine
    {
        public static string GetPropertyLine(Property property)
        {
            if (property.IsOptional)
            {
                switch (property.Type)
                {
                    case PropertyType.ByteArray:
                    case PropertyType.DateTime:
                    case PropertyType.Double:
                        return $"      {property.Name.LowerFirstChar()}: {{ }},";

                    case PropertyType.Integer:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.pattern(integerRegex)],\n" +
                               $"      }},";

                    case PropertyType.Guid:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.pattern(guidRegex)],\n" +
                               $"      }},";

                    case PropertyType.String:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.maxLength({property.TypeExtra})],\n" +
                               $"      }},";

                    case PropertyType.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        initialValue: false,\n" +
                               $"      }},";
                }
            }
            else
            {
                switch (property.Type)
                {
                    case PropertyType.ByteArray:
                    case PropertyType.DateTime:
                    case PropertyType.Double:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.required],\n" +
                               $"      }},";

                    case PropertyType.Integer:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.required, Validators.pattern(integerRegex)],\n" +
                               $"      }},";

                    case PropertyType.Guid:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.required, Validators.pattern(guidRegex)],\n" +
                               $"      }},";

                    case PropertyType.String:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        validators: [Validators.required, Validators.minLength(1), Validators.maxLength({property.TypeExtra})],\n" +
                               $"      }},";

                    case PropertyType.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: {{\n" +
                               $"        initialValue: false,\n" +
                               $"        validators: [Validators.required],\n" +
                               $"      }},";
                }
            }

            throw new NotImplementedException();
        }
    }
}