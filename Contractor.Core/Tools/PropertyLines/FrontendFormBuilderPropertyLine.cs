using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class FrontendFormBuilderPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options)
        {
            if (options.IsOptional)
            {
                switch (options.PropertyType)
                {
                    case PropertyTypes.Boolean:
                    case PropertyTypes.DateTime:
                    case PropertyTypes.Double:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, []),";

                    case PropertyTypes.Integer:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, [Validators.pattern(integerRegex)]),";

                    case PropertyTypes.Guid:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, [Validators.pattern(guidRegex)]),";

                    case PropertyTypes.String:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl('', [Validators.maxLength({options.PropertyTypeExtra})]),";
                }
            }
            else
            {
                switch (options.PropertyType)
                {
                    case PropertyTypes.Boolean:
                    case PropertyTypes.DateTime:
                    case PropertyTypes.Double:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, [Validators.required]),";

                    case PropertyTypes.Integer:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(integerRegex)]),";

                    case PropertyTypes.Guid:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl(null, [Validators.required, Validators.pattern(guidRegex)]),";

                    case PropertyTypes.String:
                        return $"      {options.PropertyName.LowerFirstChar()}: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength({options.PropertyTypeExtra})]),";
                }
            }

            throw new NotImplementedException();
        }
    }
}