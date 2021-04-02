using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendEntityTestValuesPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options, string postfix)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return "\"" + options.PropertyName + postfix + "\"";

                case PropertyTypes.Integer:
                    return new Random().Next(100, 999).ToString();

                case PropertyTypes.Double:
                    return new Random().Next(10, 99).ToString() + "." + new Random().Next(0, 99999).ToString();

                case PropertyTypes.Guid:
                    return $"Guid.Parse(\"{Guid.NewGuid()}\")";

                case PropertyTypes.Boolean:
                    return postfix.Equals("DbDefault").ToString().ToLower();

                case PropertyTypes.DateTime:
                    Random gen = new Random();
                    int range = 10 * 365; // 10 years
                    var randomDate = DateTime.Today.AddDays(-gen.Next(range));
                    return $"new DateTime({randomDate.Year}, {randomDate.Month}, {randomDate.Day})";
            }
        }
    }
}