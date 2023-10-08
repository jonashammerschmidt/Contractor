using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.CLI.Migration
{
    internal static class ContractorMigrator
    {
        public static void Migrate(ContractorXml contractorXml, IContractorOptions options, string[] args)
        {
            switch (args[1])
            {
                case "domain":
                    DomainAdditionOptions domainOptions = DomainAdditionOptionParser.ParseOptions(options, args);
                    contractorXml.Modules.Modules.Add(new ModuleXml()
                    {
                        Name = domainOptions.Domain,
                        Entities = new List<EntityXml>(),
                    });

                    break;

                case "entity":
                    EntityAdditionOptions entityOptions = EntityAdditionOptionParser.ParseOptions(options, args);
                    ModuleXml entityModule = contractorXml.Modules.Modules
                        .Single(module => module.Name == entityOptions.Domain);
                    entityModule.Entities.Add(new EntityXml()
                    {
                        Name = entityOptions.EntityName,
                        NamePlural = entityOptions.EntityNamePlural,
                        ScopeEntityName = entityOptions.RequestScopeName,
                        Properties = new List<PropertyXml>(),
                        Relation1ToN = new List<Relation1ToNXml>(),
                        Relations1To1 = new List<Relation1To1Xml>(),
                    });

                    break;

                case "property":
                    IPropertyAdditionOptions propertyOptions = PropertyAdditionOptionParser.ParseOptions(options, args);

                    ModuleXml propertyModule = contractorXml.Modules.Modules
                        .Single(module => module.Name == propertyOptions.Domain);
                    EntityXml propertyEntity = propertyModule.Entities
                        .Single(entity => entity.Name == propertyOptions.EntityName);
                    propertyEntity.Properties.Add(new PropertyXml()
                    {
                        Name = propertyOptions.PropertyName,
                        Type = propertyOptions.PropertyType + (propertyOptions.PropertyTypeExtra ?? string.Empty),
                        IsOptional = propertyOptions.IsOptional,
                    });

                    break;

                case "relation":
                    switch (args[2])
                    {
                        case "1:1":
                            IRelationAdditionOptions oneToOneRelationOptions = RelationOneToOneAdditionOptionParser.ParseOptions(options, args);

                            ModuleXml oneToOneRelationModule = contractorXml.Modules.Modules
                                .Single(module => module.Name == oneToOneRelationOptions.DomainTo);
                            EntityXml oneToOneRelationEntity = oneToOneRelationModule.Entities
                                .Single(entity => entity.Name == oneToOneRelationOptions.EntityNameTo);

                            oneToOneRelationEntity.Relations1To1.Add(new Relation1To1Xml()
                            {
                                EntityNameFrom = oneToOneRelationOptions.EntityNameFrom,
                                PropertyNameFrom = oneToOneRelationOptions.PropertyNameFrom,
                                PropertyNameTo = oneToOneRelationOptions.PropertyNameTo,
                                IsOptional = oneToOneRelationOptions.IsOptional,
                            });

                            break;

                        case "1:n":
                            IRelationAdditionOptions relation1ToNAdditionOptions = Relation1ToNAdditionOptionParser.ParseOptions(options, args);
                            
                            ModuleXml oneToNRelationModule = contractorXml.Modules.Modules
                                .Single(module => module.Name == relation1ToNAdditionOptions.DomainTo);
                            EntityXml oneToNRelationEntity = oneToNRelationModule.Entities
                                .Single(entity => entity.Name == relation1ToNAdditionOptions.EntityNameTo);

                            oneToNRelationEntity.Relation1ToN.Add(new Relation1ToNXml()
                            {
                                EntityNameFrom = relation1ToNAdditionOptions.EntityNameFrom,
                                PropertyNameFrom = relation1ToNAdditionOptions.PropertyNameFrom,
                                PropertyNameTo = relation1ToNAdditionOptions.PropertyNameTo,
                                IsOptional = relation1ToNAdditionOptions.IsOptional,
                            });
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