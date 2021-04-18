using Contractor.CLI.Tools;
using Contractor.Core;
using Contractor.Core.Options;
using System;
using System.IO;

namespace Contractor.CLI
{
    internal class Relation1ToNAdditionHandler
    {
        public static void Perform(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine("Bitte geben sie alle Informationen an. Beispiel: contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden [-n|--alternative-property-names Vertragsbank:Vertragskunden]");
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

            if (ArgumentParser.HasArgument(args, "-n", "--alternative-property-names"))
            {
                string st = ArgumentParser.ExtractArgument(args, "-n", "--alternative-property-names");
                options.PropertyNameFrom = st.Split(':')[0];
                options.PropertyNameTo = st.Split(':')[1];
            }
            else
            {
                options.PropertyNameFrom = options.EntityNameFrom;
                options.PropertyNameTo = options.EntityNamePluralTo;
            }
        }
    }
};