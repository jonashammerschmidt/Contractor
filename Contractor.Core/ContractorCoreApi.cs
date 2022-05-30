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
        private readonly IFileSystemClient fileSystemClient;
        private readonly List<ClassGeneration> classGenerations = new List<ClassGeneration>();

        public ContractorCoreApi()
        {
            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();

            this.fileSystemClient = serviceProvider.GetRequiredService<IFileSystemClient>();
            this.classGenerations = serviceProvider.GetServices<ClassGeneration>().ToList();
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            options = new DomainAdditionOptions(options);
            if (!DomainAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                if (ShouldGenerate(options, classGeneration))
                {
                    classGeneration.PerformAddDomainCommand(options);
                }
            }
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            options = new EntityAdditionOptions(options);
            if (!EntityAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                if (ShouldGenerate(options, classGeneration))
                {
                    classGeneration.PerformAddEntityCommand(options);
                }
            }
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            options = new PropertyAdditionOptions(options);
            if (!PropertyAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                if (ShouldGenerate(options, classGeneration))
                {
                    classGeneration.PerformAddPropertyCommand(options);
                }
            }
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            options = new RelationAdditionOptions(options);
            if (!RelationAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                if (ShouldGenerate(options, classGeneration))
                {
                    classGeneration.PerformAdd1ToNRelationCommand(options);
                }
            }
        }

        public void AddOneToOneRelation(IRelationAdditionOptions options)
        {
            options = new RelationAdditionOptions(options);
            if (!RelationAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                if (ShouldGenerate(options, classGeneration))
                {
                    classGeneration.PerformAddOneToOneRelationCommand(options);
                }
            }
        }

        public void SaveChanges(IContractorOptions contractorOptions)
        {
            this.fileSystemClient.SaveAll(contractorOptions);
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