using Contractor.Core.MetaModell;
using System;
using System.Globalization;
using System.Text;

namespace Contractor.Core.Tools
{
    public static class TestValueGeneration
    {
        public static string GetPropertyLine(Property property, string postfix, Random random)
        {
            return GetPropertyLine(property, string.Empty, postfix, random);
        }

        public static string GetPropertyLine(Property property, string prefix, string postfix, Random random)
        {
            switch (property.Type)
            {
                case PropertyType.String:
                    var length = Math.Min(
                        property.Name.Length,
                        Math.Max(0, int.Parse(property.TypeExtra) - prefix.Length - postfix.Length));
                    return "\"" + prefix + property.Name.Substring(0, length) + postfix + "\"";

                case PropertyType.Integer:
                    return random.Next(100, 999).ToString();

                case PropertyType.Double:
                    return random.Next(10, 99).ToString() + "." + random.Next(0, 99999).ToString();

                case PropertyType.Guid:
                    return "\"" + GenerateGuid(random) + "\"";

                case PropertyType.Boolean:
                    return postfix.Equals("DbDefault").ToString().ToLower();

                case PropertyType.DateTime:
                    Random gen = random;
                    int range = 10 * 365; // 10 years
                    var randomDate = new DateTime(2020, 12, 31).AddDays(-gen.Next(range));
                    return "\"" + randomDate.ToString("yyyy-MM-ddTHH:mmZ", CultureInfo.InvariantCulture) + "\"";

                case PropertyType.ByteArray:
                    return "\"" + Convert.ToBase64String(Encoding.UTF8.GetBytes(prefix + property.Name + postfix)) + "\"";

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