using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendEntityTestValuesPropertyLine
    {
        public static string GetPropertyLine(Property property, string postfix, Random random)
        {
            switch (property.Type)
            {
                case PropertyTypes.String:
                    return "\"" + property.Name + postfix + "\"";

                case PropertyTypes.Integer:
                    return random.Next(100, 999).ToString();

                case PropertyTypes.Double:
                    return random.Next(10, 99).ToString() + "." + random.Next(0, 99999).ToString();

                case PropertyTypes.Guid:
                    var guid = new byte[16];
                    random.NextBytes(guid);
                    return $"Guid.Parse(\"{new Guid(guid)}\")";

                case PropertyTypes.Boolean:
                    return postfix.Equals("DbDefault").ToString().ToLower();

                case PropertyTypes.DateTime:
                    Random gen = random;
                    int range = 10 * 365; // 10 years
                    var randomDate = new DateTime(2020, 12, 31).AddDays(-gen.Next(range));
                    return $"new DateTime({randomDate.Year}, {randomDate.Month}, {randomDate.Day})";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}