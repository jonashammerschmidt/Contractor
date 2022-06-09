using Contractor.Core.Options;
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

            foreach (var module in this.contractorGenerationOptions.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    foreach (var classGeneration in this.classGenerations)
                    {
                        classGeneration.PerformAddEntityCommand(entity);
                    }

                    var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var property = entity.Properties.Single(p => p.Order == i);
                        if (property != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                //classGeneration.PerformAddPropertyCommand
                            }
                        }

                        var relation1To1 = entity.Relations1To1.Single(p => p.Order == i);
                        if (relation1To1 != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                // TO classGeneration.PerformAdd1To1RelationCommand
                            }
                        }

                        var relation1ToN = entity.Relations1ToN.Single(p => p.Order == i);
                        if (relation1ToN != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                // TO classGeneration.PerformAdd1ToNRelationCommand
                            }
                        }
                    }

                }
            }


            foreach (var module in this.contractorGenerationOptions.Modules)
            {
                foreach (var entity in module.Entities)
                {
                    var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var relation1To1 = entity.Relations1To1.Single(p => p.Order == i);
                        if (relation1To1 != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                // FROM classGeneration.PerformAdd1To1RelationCommand
                            }
                        }

                        var relation1ToN = entity.Relations1ToN.Single(p => p.Order == i);
                        if (relation1ToN != null)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                // FROM classGeneration.PerformAdd1ToNRelationCommand
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

        private bool ShouldGenerate(IContractorOptions options, ClassGeneration classGeneration)
        {
            if (options.Tags.Count() == 0)
            {
                return true;
            }

            return classGeneration
               .GetType()
               .GetCustomAttributes(typeof(ClassGenerationTagsAttribute), false)
               .Select(attribute => attribute as ClassGenerationTagsAttribute)
               .SelectMany(attribute => attribute.GetGenerationTags())
               .Any(tag => options.Tags.Contains(tag));
        }
    }
}