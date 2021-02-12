using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.Options;
using System;
using System.IO;

namespace Contractor.CLI
{
    internal class PropertyAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Bitte geben sie einen Domain Name an: contractor add property string:256 Name -e Bankwesen.Bank:Banken");
                return;
            }

            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            var propertyOptions = new PropertyAdditionOptions(options);
            ParseOptions(propertyOptions, args);

            try
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
                contractorCoreApi.AddProperty(propertyOptions);
                Console.WriteLine($"Property '{propertyOptions.PropertyName}' zur Entity '{propertyOptions.EntityName}' hinzugefügt'");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ParseOptions(IPropertyAdditionOptions options, string[] args)
        {
            options.PropertyType = args[2].Split(':')[0];
            if (args[2].Contains(':'))
            {
                options.PropertyTypeExtra = args[2].Split(':')[1];
            }

            options.PropertyName = args[3];

            string entityName = ArgumentParser.ExtractArgument(args, "-e", "--entity");
            options.Domain = entityName.Split('.')[0];
            options.EntityName = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePlural = entityName.Split(':')[1];
        }
    }
}