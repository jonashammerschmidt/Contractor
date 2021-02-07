using Contractor.Core;
using Contractor.Core.Helpers;
using Contractor.Core.Jobs;
using System;
using System.IO;
using System.Linq;

namespace Contractor.CLI
{
    public class PropertyAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Bitte geben sie einen Domain Name an: contractor add property string?(256) KundenId -e Finanzen.Bankwesen.Bank");
                return;
            }

            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());

            var propertyOptions = new PropertyOptions(options);

            ParseOptions(propertyOptions, args);
            if (ValidateOptions(propertyOptions))
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

                try
                {
                    contractorCoreApi.AddProperty(propertyOptions);
                    Console.WriteLine($"Property '{propertyOptions.PropertyName}' zur Entity '{propertyOptions.EntityName}' hinzugefügt'");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine("Die Argumente sind nicht korrekt.");
            }
        }

        private static void ParseOptions(PropertyOptions options, string[] args)
        {
            options.PropertyType = args[2].Split('(')[0];
            if (args[2].Contains('('))
            {
                options.PropertyTypeExtra = args[2].Split('(')[1].Split(')')[0];
            }

            options.PropertyName = args[3];

            string entityName = ExtractArgument(args, "-e", "--entity");
            if (entityName != null)
            {
                if (entityName.Contains('.'))
                {
                    options.Domain = entityName.Split('.')[0];
                    options.EntityName = entityName.Split('.')[1].Split(':')[0];
                }
                else
                {
                    options.EntityName = entityName.Split(':')[0];
                }

                if (entityName.Contains(':'))
                {
                    options.EntityNamePlural = entityName.Split(':')[1];
                }
            }

            options.Domain = ExtractArgument(args, "-d", "--domain") ?? options.Domain;
            options.EntityNamePlural = ExtractArgument(args, "--plural") ?? options.EntityNamePlural;
            options.PropertyTypeExtra = ExtractArgument(args, "--extra") ?? options.PropertyTypeExtra;
        }

        private static string ExtractArgument(string[] args, params string[] argumentAlternatives)
        {
            int index = args.FindIndex((arg) => argumentAlternatives.Contains(arg));
            if (index == -1 || args.Length <= index + 1)
            {
                return null;
            }

            return args[index + 1];
        }

        private static bool ValidateOptions(PropertyOptions options)
        {
            if (string.IsNullOrEmpty(options.Domain) ||
               string.IsNullOrEmpty(options.EntityName) ||
               string.IsNullOrEmpty(options.EntityNamePlural) ||
               string.IsNullOrEmpty(options.PropertyName) ||
               string.IsNullOrEmpty(options.PropertyType) ||
               !options.Domain.IsAlpha() ||
               !options.EntityName.IsAlpha() ||
               !options.EntityNamePlural.IsAlpha() ||
               !options.PropertyName.IsAlpha())
            {
                return false;
            }

            options.Domain = options.Domain.UpperFirstChar();
            options.EntityName = options.EntityName.UpperFirstChar();
            options.EntityNamePlural = options.EntityNamePlural.UpperFirstChar();
            options.PropertyName = options.PropertyName.UpperFirstChar();

            return true;
        }
    }
}