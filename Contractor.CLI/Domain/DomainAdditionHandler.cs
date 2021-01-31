using Contractor.Core;
using Contractor.Core.Helpers;
using Contractor.Core.Jobs.DomainAddition;
using System;
using System.IO;

namespace Contractor.CLI
{
    public class DomainAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Bitte geben sie einen Domain Name an: contractor add domain <domain-name>");
                return;
            }

            DomainOptions domainOptions = GetOptions(args);
            AddDomain(domainOptions);
        }

        private static DomainOptions GetOptions(string[] args)
        {
            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            var domainOptions = new DomainOptions(options)
            {
                Domain = args[2]
            };
            domainOptions.Domain = domainOptions.Domain.UpperFirstChar();
            return domainOptions;
        }

        private static void AddDomain(DomainOptions domainOptions)
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