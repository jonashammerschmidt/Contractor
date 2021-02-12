using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.Options;
using System;
using System.IO;

namespace Contractor.CLI
{
    internal class EntityAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Bitte geben sie alle Informationen an. Beispiel: contractor add entity Bankwesen.Bank:Banken [-m | --for-mandant]");
                return;
            }

            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            EntityAdditionOptions entityOptions = new EntityAdditionOptions(options);
            ParseOptions(entityOptions, args);

            try
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
                contractorCoreApi.AddEntity(entityOptions);
                Console.WriteLine($"Entity '{entityOptions.EntityName} ({entityOptions.EntityNamePlural})' zur Domain '{entityOptions.Domain}' hinzugefügt'");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ParseOptions(IEntityAdditionOptions options, string[] args)
        {
            string entityName = args[2];
            options.Domain = entityName.Split('.')[0];
            options.EntityName = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePlural = entityName.Split(':')[1];
            options.ForMandant = ArgumentParser.HasArgument(args, "-m", "--for-mandant");
        }
    }
};