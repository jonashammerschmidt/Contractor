using Contractor.Core.Projects;
using Contractor.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class ContractorCoreApi
    {
        private readonly ContractorGenerationOptions contractorGenerationOptions;

        private readonly IFileSystemClient fileSystemClient;
        private readonly List<ClassGeneration> classGenerations = new List<ClassGeneration>();

        public ContractorCoreApi(ContractorGenerationOptions contractorGenerationOptions)
        {
            this.contractorGenerationOptions = contractorGenerationOptions;

            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();
            this.fileSystemClient = serviceProvider.GetRequiredService<IFileSystemClient>();
            this.classGenerations = serviceProvider.GetServices<ClassGeneration>().ToList();
        }

        public void Generate()
        {
            foreach (var module in this.contractorGenerationOptions.Modules)
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    classGeneration.AddModule(module);
                }
            }

            foreach (Module module in this.contractorGenerationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (Entity entity in module.Entities.Where(entity => !entity.Skip))
                {
                    foreach (var classGeneration in this.classGenerations)
                    {
                        classGeneration.PerformAddEntityCommand(entity);
                    }

                    var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                    for (int i = 0; i < count; i++)
                    {
                        Property property = entity.Properties.SingleOrDefault(p => p.Order == i);
                        if (property != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                classGeneration.PerformAddPropertyCommand(property);
                            }
                        }

                        Relation1To1 relation1To1 = entity.Relations1To1.SingleOrDefault(p => p.Order == i);
                        if (relation1To1 != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                classGeneration.PerformAddOneToOneRelationSideToCommand(relation1To1);
                            }
                        }

                        Relation1ToN relation1ToN = entity.Relations1ToN.SingleOrDefault(p => p.Order == i);
                        if (relation1ToN != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                classGeneration.PerformAdd1ToNRelationSideToCommand(relation1ToN);
                            }
                        }
                    }

                }
            }


            foreach (var module in this.contractorGenerationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (var entity in module.Entities.Where(entity => !entity.Skip))
                {
                    var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var relation1To1 = entity.Relations1To1.SingleOrDefault(p => p.Order == i);
                        if (relation1To1 != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                classGeneration.PerformAddOneToOneRelationSideFromCommand(relation1To1);
                            }
                        }

                        var relation1ToN = entity.Relations1ToN.SingleOrDefault(p => p.Order == i);
                        if (relation1ToN != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                classGeneration.PerformAdd1ToNRelationSideFromCommand(relation1ToN);
                            }
                        }
                    }
                }
            }
        }

        public void SaveChanges()
        {
            this.fileSystemClient.SaveAll(contractorGenerationOptions);
        }

        //private bool ShouldGenerate(ContractorGenerationOptions options, ClassGeneration classGeneration)
        //{
        //    if (options.Tags.Count() == 0)
        //    {
        //        return true;
        //    }

        //    return classGeneration
        //       .GetType()
        //       .GetCustomAttributes(typeof(ClassGenerationTagsAttribute), false)
        //       .Select(attribute => attribute as ClassGenerationTagsAttribute)
        //       .SelectMany(attribute => attribute.GetGenerationTags())
        //       .Any(tag => options.Tags.Contains(tag));
        //}
    }
}