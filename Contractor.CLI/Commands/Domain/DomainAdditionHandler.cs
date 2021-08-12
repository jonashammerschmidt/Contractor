using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.Helpers;
using Contractor.Core.Options;
using System;
using System.IO;

namespace Contractor.CLI
{
    internal class DomainAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Bitte geben sie einen Domain Name an. Beispiel: contractor add domain Bankwesen");
                return;
            }

            DomainAdditionOptions domainOptions = GetOptions(args);
            AddDomain(domainOptions);
        }

        private static DomainAdditionOptions GetOptions(string[] args)
        {
            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            var domainOptions = new DomainAdditionOptions(options);

            domainOptions.IsVerbose = ArgumentParser.HasArgument(args, "-v", "--verbose");
            
            domainOptions.Domain = args[2].UpperFirstChar();

            return domainOptions;
        }

        private static void AddDomain(IDomainAdditionOptions domainOptions)
        {
            try
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
                contractorCoreApi.AddDomain(domainOptions);
                Console.WriteLine($"Domain '{domainOptions.Domain}' hinzugefügt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}