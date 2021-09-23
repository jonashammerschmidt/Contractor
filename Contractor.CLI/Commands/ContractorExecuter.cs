using Contractor.Core;
using Contractor.Core.Options;
using System;

namespace Contractor.CLI
{
    internal static class ContractorExecuter
    {
        public static void Execute(ContractorCoreApi contractorCoreApi, IContractorOptions options, string[] args)
        {
            switch (args[1])
            {
                case "domain":
                    contractorCoreApi.AddDomain(DomainAdditionOptionParser.ParseOptions(options, args));
                    break;

                case "entity":
                    contractorCoreApi.AddEntity(EntityAdditionOptionParser.ParseOptions(options, args));
                    break;

                case "property":
                    contractorCoreApi.AddProperty(PropertyAdditionOptionParser.ParseOptions(options, args));
                    break;

                case "relation":
                    switch (args[2])
                    {
                        case "1:1":
                            contractorCoreApi.AddOneToOneRelation(RelationOneToOneAdditionOptionParser.ParseOptions(options, args));
                            break;

                        case "1:n":
                            contractorCoreApi.Add1ToNRelation(Relation1ToNAdditionOptionParser.ParseOptions(options, args));
                            break;

                        default:
                            Console.WriteLine($"Relation type {args[2]} not found");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Der Typ '" + args[1] + "' ist ungültig.");
                    Console.WriteLine("Benutze 'contractor help' um die Hilfe anzuzeigen.");
                    break;
            }
        }
    }
}