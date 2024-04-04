using Contractor.Core.Generation;
using Contractor.Core.MetaModell;
using Contractor.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Contractor.Core.Generation.Backend.Generated.DTOs;
using Contractor.Core.Generation.Backend.Persistence;

namespace Contractor.Core
{
    public class GenerationFacade
    {
        private readonly GenerationOptions generationOptions;
        private readonly IEnumerable<Entity> sortedEntities;

        private readonly IFileSystemClient fileSystemClient;
        private readonly List<ClassGeneration> classGenerations = new();
        
        private readonly EntityDtoForPurposeGeneration entityDtoForPurposeGeneration;
        private readonly EntitiesCrudRepositoryGeneration entitiesCrudRepositoryGeneration;
        private readonly IEntitiesCrudRepositoryGeneration iEntitiesCrudRepositoryGeneration;

        public GenerationFacade(GenerationOptions generationOptions)
        {
            this.generationOptions = generationOptions;
            this.sortedEntities = GenerationPreprocessor.PreProcess(this.generationOptions);

            IServiceCollection serviceCollection = new ServiceCollection();
            GenerationDependencyProvider.ConfigureServices(serviceCollection);
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            this.fileSystemClient = serviceProvider.GetRequiredService<IFileSystemClient>();
            this.classGenerations = serviceProvider.GetServices<ClassGeneration>().ToList();

            this.entityDtoForPurposeGeneration = serviceProvider.GetService<EntityDtoForPurposeGeneration>();
            this.entitiesCrudRepositoryGeneration = serviceProvider.GetService<EntitiesCrudRepositoryGeneration>();
            this.iEntitiesCrudRepositoryGeneration = serviceProvider.GetService<IEntitiesCrudRepositoryGeneration>();
        }

        public void Generate()
        {
            foreach (var module in this.generationOptions.Modules.Where(module => !module.Skip))
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    if (ShouldGenerate(this.generationOptions.Tags, classGeneration))
                    {
                        classGeneration.AddModule(module);
                    }
                }
            }

            foreach (Entity entity in this.sortedEntities.Where(entity => !entity.Skip))
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    if (ShouldGenerate(this.generationOptions.Tags, classGeneration))
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
                            if (ShouldGenerate(this.generationOptions.Tags, classGeneration))
                            {
                                classGeneration.PerformAddPropertyCommand(property);
                            }
                        }
                    }

                    foreach (var relation1To1 in entity.Relations1To1.Where(p => p.Order == i))
                    {
                        foreach (var classGeneration in this.classGenerations)
                        {
                            if (ShouldGenerateRelation(relation1To1, classGeneration))
                            {
                                classGeneration.PerformAddOneToOneRelationSideToCommand(relation1To1);
                            }
                        }
                    }

                    foreach (var relation1ToN in entity.Relations1ToN.Where(p => p.Order == i))
                    {
                        foreach (var classGeneration in this.classGenerations)
                        {
                            if (ShouldGenerateRelation(relation1ToN, classGeneration))
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
                                if (ShouldGenerateRelation(relation1To1, classGeneration))
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
                                if (ShouldGenerateRelation(relation1ToN, classGeneration))
                                {
                                    classGeneration.PerformAdd1ToNRelationSideFromCommand(relation1ToN);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var customDto in generationOptions.CustomDtos)
            {
                entityDtoForPurposeGeneration.Generate(customDto);

                iEntitiesCrudRepositoryGeneration.AddCustomDto(customDto);
                entitiesCrudRepositoryGeneration.AddCustomDto(customDto);
            }

            foreach (Entity entity in this.sortedEntities.Where(entity => !entity.Skip))
            {
                foreach (var classGeneration in this.classGenerations)
                {
                    if (ShouldGenerate(this.generationOptions.Tags, classGeneration))
                    {
                        classGeneration.PerformPostGenerationCommand(entity);
                    }
                }
            }
        }

        public void SaveChanges()
        {
            this.fileSystemClient.SaveAll(generationOptions);
        }

        private bool ShouldGenerate(IEnumerable<ClassGenerationTag> tags, ClassGeneration classGeneration)
        {
            return ShouldGenerate(tags, classGeneration, false);
        }

        private bool ShouldGenerate(IEnumerable<ClassGenerationTag> tags, ClassGeneration classGeneration, bool strict)
        {
            if (!strict && (tags == null || tags.Count() == 0))
            {
                return true;
            }

            return classGeneration
               .GetType()
               .GetCustomAttributes(typeof(ClassGenerationTagsAttribute), false)
               .Select(attribute => attribute as ClassGenerationTagsAttribute)
               .SelectMany(attribute => attribute.GetGenerationTags())
               .Any(tag => tags.Contains(tag));
        }

        private bool ShouldGenerateRelation(Relation relation, ClassGeneration classGeneration)
        {
            if (!relation.IsCreatedByPreProcessor)
            {
                return ShouldGenerate(relation.EntityTo.Module.Options.Tags, classGeneration);
            }

            IEnumerable<ClassGenerationTag> tagIntersection = new List<ClassGenerationTag>()
            {
                ClassGenerationTag.BACKEND_MISC,
                ClassGenerationTag.BACKEND_PERSISTENCE_DB_CONTEXT,
                ClassGenerationTag.BACKEND_PERSISTENCE_REPOSITORY,
            };

            if (relation.EntityTo.Module.Options.Tags != null && relation.EntityTo.Module.Options.Tags.Count() > 0)
            {
                tagIntersection = tagIntersection.Intersect(relation.EntityTo.Module.Options.Tags);
            }

            return ShouldGenerate(tagIntersection, classGeneration, true);
        }
    }
}