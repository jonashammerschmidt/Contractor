using Contractor.Core.Helpers;
using Contractor.Core.Options;
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
                    case PropertyTypes.DateTime:
                    case PropertyTypes.Double:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, []),";

                    case PropertyTypes.Integer:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.pattern(integerRegex)]),";

                    case PropertyTypes.Guid:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.pattern(guidRegex)]),";

                    case PropertyTypes.String:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl('', [Validators.maxLength({property.TypeExtra})]),";

                    case PropertyTypes.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(false, []),";
                }
            }
            else
            {
                switch (property.Type)
                {
                    case PropertyTypes.DateTime:
                    case PropertyTypes.Double:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required]),";

                    case PropertyTypes.Integer:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(integerRegex)]),";

                    case PropertyTypes.Guid:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(guidRegex)]),";

                    case PropertyTypes.String:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength({property.TypeExtra})]),";

                    case PropertyTypes.Boolean:
                        return $"      {property.Name.LowerFirstChar()}: new FormControl(false, [Validators.required]),";
                }
            }

            throw new NotImplementedException();
        }
    }
}