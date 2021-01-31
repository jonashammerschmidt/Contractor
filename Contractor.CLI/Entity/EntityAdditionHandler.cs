using Contractor.Core;
using Contractor.Core.Helpers;
using Contractor.Core.Jobs.EntityAddition;
using System;
using System.IO;
using System.Linq;

namespace Contractor.CLI
{
    public class EntityAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Bitte geben sie alle Informationen an. Beispiel: contractor add entity <entity-name-or-path>[:<entity-name-plural>] [--domain <domain-name>] [--plural <entity-name-plural>]");
                return;
            }

            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            EntityOptions entityOptions = new EntityOptions(options);
            ParseOptions(entityOptions, args);
            if (ValidateOptions(entityOptions))
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();

                try
                {
                    contractorCoreApi.AddEntity(entityOptions);
                    Console.WriteLine($"Entity '{entityOptions.EntityName} ({entityOptions.EntityNamePlural})' zur Domain '{entityOptions.Domain}' hinzugefügt'");
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

        private static void ParseOptions(EntityOptions options, string[] args)
        {
            string entityName = args[2];

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

            options.Domain = ExtractArgument(args, "-d", "--domain") ?? options.Domain;
            options.EntityNamePlural = ExtractArgument(args, "-p", "--plural") ?? options.EntityNamePlural;
            options.ForMandant = HasArgument(args, "-m", "--mandant", "--for-mandant");
        }

        private static bool HasArgument(string[] args, params string[] argumentAlternatives)
        {
            int index = args.FindIndex((arg) => argumentAlternatives.Contains(arg));
            return index != -1;
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

        private static bool ValidateOptions(EntityOptions options)
        {
            if (string.IsNullOrEmpty(options.Domain) ||
               string.IsNullOrEmpty(options.EntityName) ||
               string.IsNullOrEmpty(options.EntityNamePlural) ||
               !options.Domain.IsAlpha() ||
               !options.EntityName.IsAlpha() ||
               !options.EntityNamePlural.IsAlpha())
            {
                return false;
            }

            options.Domain = options.Domain.UpperFirstChar();
            options.EntityName = options.EntityName.UpperFirstChar();
            options.EntityNamePlural = options.EntityNamePlural.UpperFirstChar();

            return true;
        }
    }
};