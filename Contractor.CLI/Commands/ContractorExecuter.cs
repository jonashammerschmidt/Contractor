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
                    DomainAdditionOptions domainOptions = DomainAdditionOptionParser.ParseOptions(options, args);
                    contractorCoreApi.AddDomain(domainOptions);

                    if (options.IsVerbose)
                    {
                        Console.WriteLine($"Domain '{domainOptions.Domain}' hinzugefügt.");
                    }

                    break;

                case "entity":
                    EntityAdditionOptions entityOptions = EntityAdditionOptionParser.ParseOptions(options, args);
                    contractorCoreApi.AddEntity(entityOptions);

                    if (options.IsVerbose)
                    {
                        Console.WriteLine($"Entity '{entityOptions.EntityName} ({entityOptions.EntityNamePlural})' zur Domain '{entityOptions.Domain}' hinzugefügt'");
                    }

                    break;

                case "property":
                    IPropertyAdditionOptions propertyOptions = PropertyAdditionOptionParser.ParseOptions(options, args);
                    contractorCoreApi.AddProperty(propertyOptions);

                    if (options.IsVerbose)
                    {
                        Console.WriteLine($"Property '{propertyOptions.PropertyName}' zur Entity '{propertyOptions.EntityName}' hinzugefügt'");
                    }

                    break;

                case "relation":
                    switch (args[2])
                    {
                        case "1:1":
                            IRelationAdditionOptions oneToOneRelationOptions = RelationOneToOneAdditionOptionParser.ParseOptions(options, args);
                            contractorCoreApi.AddOneToOneRelation(oneToOneRelationOptions);

                            if (options.IsVerbose)
                            {
                                Console.WriteLine($"1-zu-1 Relation zwischen '{oneToOneRelationOptions.EntityNameFrom}' und '{oneToOneRelationOptions.EntityNameTo}' hinzugefügt");
                            }

                            break;

                        case "1:n":
                            IRelationAdditionOptions relation1ToNAdditionOptions = Relation1ToNAdditionOptionParser.ParseOptions(options, args);
                            contractorCoreApi.Add1ToNRelation(relation1ToNAdditionOptions);


                            if (options.IsVerbose)
                            {
                                Console.WriteLine($"1-zu-N Relation zwischen '{relation1ToNAdditionOptions.EntityNameFrom}' und '{relation1ToNAdditionOptions.EntityNamePluralTo}' hinzugefügt");
                            }


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