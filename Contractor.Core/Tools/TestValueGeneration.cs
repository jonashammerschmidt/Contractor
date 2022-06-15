using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class TestValueGeneration
    {
        public static string GetPropertyLine(Property property, string postfix, Random random)
        {
            switch (property.Type)
            {
                case PropertyTypes.String:
                    return property.Name + postfix;

                case PropertyTypes.Integer:
                    return random.Next(100, 999).ToString();

                case PropertyTypes.Double:
                    return random.Next(10, 99).ToString() + "." + random.Next(0, 99999).ToString();

                case PropertyTypes.Guid:
                    return GenerateGuid(random);

                case PropertyTypes.Boolean:
                    return postfix.Equals("DbDefault").ToString().ToLower();

                case PropertyTypes.DateTime:
                    Random gen = random;
                    int range = 10 * 365; // 10 years
                    var randomDate = new DateTime(2020, 12, 31).AddDays(-gen.Next(range));
                    return randomDate.ToUniversalTime().ToString();

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GenerateGuid(Random random)
        {
            var guid = new byte[16];
            random.NextBytes(guid);
            return new Guid(guid).ToString();
        }
    }
}