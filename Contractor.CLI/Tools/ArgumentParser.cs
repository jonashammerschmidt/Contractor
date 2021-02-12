using Contractor.Core.Helpers;
using System;
using System.Linq;

namespace Contractor.CLI.Tools
{
    internal static class ArgumentParser
    {
        public static bool HasArgument(string[] args, params string[] argumentAlternatives)
        {
            int index = args.FindIndex((arg) => argumentAlternatives.Contains(arg));
            return index != -1;
        }

        public static string ExtractArgument(string[] args, params string[] argumentAlternatives)
        {
            int index = args.FindIndex((arg) => argumentAlternatives.Contains(arg));
            if (index == -1 || args.Length <= index + 1)
            {
                return null;
            }

            return args[index + 1];
        }
    }
}