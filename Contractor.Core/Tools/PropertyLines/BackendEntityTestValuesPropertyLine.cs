﻿using Contractor.Core.Options;
using System;

namespace Contractor.Core.Tools
{
    public static class BackendEntityTestValuesPropertyLine
    {
        public static string GetPropertyLine(IPropertyAdditionOptions options, string postfix, Random random)
        {
            switch (options.PropertyType)
            {
                case PropertyTypes.String:
                    return "\"" + options.PropertyName + postfix + "\"";

                case PropertyTypes.Integer:
                    return random.Next(100, 999).ToString();

                case PropertyTypes.Double:
                    return random.Next(10, 99).ToString() + "." + random.Next(0, 99999).ToString();

                case PropertyTypes.Guid:
                    return $"Guid.Parse(\"{Guid.NewGuid()}\")";

                case PropertyTypes.Boolean:
                    return postfix.Equals("DbDefault").ToString().ToLower();

                case PropertyTypes.DateTime:
                    Random gen = random;
                    int range = 10 * 365; // 10 years
                    var randomDate = DateTime.Today.AddDays(-gen.Next(range));
                    return $"new DateTime({randomDate.Year}, {randomDate.Month}, {randomDate.Day})";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}