using Contractor.Core;
using Contractor.Core.Jobs;
using System;
using System.IO;

namespace Contractor.CLI
{
    public class Relation1ToNAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine("Bitte geben sie alle Informationen an. Beispiel: contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden");
                return;
            }

            var options = ContractorOptionsLoader.Load(Directory.GetCurrentDirectory());
            RelationAdditionOptions relationOptions = new RelationAdditionOptions(options);
            ParseOptions(relationOptions, args);

            try
            {
                ContractorCoreApi contractorCoreApi = new ContractorCoreApi();
                contractorCoreApi.Add1ToNRelation(relationOptions);
                Console.WriteLine($"1-zu-N Relation zwischen '{relationOptions.EntityNameFrom}' und '{relationOptions.EntityNamePluralTo}' hinzugefügt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ParseOptions(IRelationAdditionOptions options, string[] args)
        {
            string entityName = args[3];
            options.DomainFrom = entityName.Split('.')[0];
            options.EntityNameFrom = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePluralFrom = entityName.Split(':')[1];

            entityName = args[4];
            options.DomainTo = entityName.Split('.')[0];
            options.EntityNameTo = entityName.Split('.')[1].Split(':')[0];
            options.EntityNamePluralTo = entityName.Split(':')[1];
        }
    }
};