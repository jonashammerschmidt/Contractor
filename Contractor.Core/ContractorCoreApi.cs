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
        private IEnumerable<Entity> sortedEntities;

        private readonly IFileSystemClient fileSystemClient;
        private readonly List<ClassGeneration> classGenerations = new List<ClassGeneration>();

        public ContractorCoreApi(ContractorGenerationOptions contractorGenerationOptions)
        {
            this.contractorGenerationOptions = contractorGenerationOptions;
            this.sortedEntities = ContractorPreprocessor.PreProcess(this.contractorGenerationOptions);

            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();
            this.fileSystemClient = serviceProvider.GetRequiredService<IFileSystemClient>();
            this.classGenerations = serviceProvider.GetServices<ClassGeneration>().ToList();
        }

        public void Generate()
        {
            foreach (var module in this.contractorGenerationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    classGeneration.AddModule(module);
                }
            }

            foreach (Entity entity in this.sortedEntities.Where(entity => !entity.Skip))
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                    {
                        classGeneration.PerformAddEntityCommand(entity);
                    }
                }

                var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                for (int i = 0; i < count; i++)
                {
                    Property property = entity.Properties.SingleOrDefault(p => p.Order == i);
                    if (property != null)
                    {
                        foreach (var classGeneration in this.classGenerations)
                        {
                            if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                            {
                                classGeneration.PerformAddPropertyCommand(property);
                            }
                        }
                    }


                    foreach (var relation1To1 in entity.Relations1To1.Where(p => p.Order == i))
                    {
                        foreach (var classGeneration in this.classGenerations)
                        {
                            if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                            {
                                classGeneration.PerformAddOneToOneRelationSideToCommand(relation1To1);
                            }
                        }
                    }

                    foreach (var relation1ToN in entity.Relations1ToN.Where(p => p.Order == i))
                    {
                        foreach (var classGeneration in this.classGenerations)
                        {
                            if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                            {
                                classGeneration.PerformAdd1ToNRelationSideToCommand(relation1ToN);
                            }
                        }
                    }
                }
            }

            foreach (var entity in this.sortedEntities.Where(entity => !entity.Skip))
            {
                var count = entity.Properties.Count() + entity.Relations1To1.Count() + entity.Relations1ToN.Count();
                for (int i = 0; i < count; i++)
                {
                    foreach (var relation1To1 in entity.Relations1To1.Where(p => p.Order == i))
                    {
                        if (relation1To1 != null && !relation1To1.EntityFrom.Skip)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                                {
                                    classGeneration.PerformAddOneToOneRelationSideFromCommand(relation1To1);
                                }
                            }
                        }
                    }
                    foreach (var relation1ToN in entity.Relations1ToN.Where(p => p.Order == i))
                    {
                        if (relation1ToN != null && !relation1ToN.EntityFrom.Skip)
                        {
                            foreach (var classGeneration in this.classGenerations)
                            {
                                if (ShouldGenerate(this.contractorGenerationOptions, classGeneration))
                                {
                                    classGeneration.PerformAdd1ToNRelationSideFromCommand(relation1ToN);
                                }
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

        private bool ShouldGenerate(ContractorGenerationOptions options, ClassGeneration classGeneration)
        {
            if (options.Tags == null || options.Tags.Count() == 0)
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