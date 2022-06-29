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
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, []),";

                    case PropertyType.Integer:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.pattern(integerRegex)]),";

                    case PropertyType.Guid:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.pattern(guidRegex)]),";

                    case PropertyType.String:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl('', [Validators.maxLength({property.TypeExtra})]),";

                    case PropertyType.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(false, []),";
                }
            }
            else
            {
                switch (property.Type)
                {
                    case PropertyType.ByteArray:
                    case PropertyType.DateTime:
                    case PropertyType.Double:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required]),";

                    case PropertyType.Integer:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(integerRegex)]),";

                    case PropertyType.Guid:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(guidRegex)]),";

                    case PropertyType.String:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength({property.TypeExtra})]),";

                    case PropertyType.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(false, [Validators.required]),";
                }
            }

            throw new NotImplementedException();
        }
    }
}