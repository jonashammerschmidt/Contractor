using Contractor.Core.Options;
using Contractor.Core.Projects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core
{
    public class ContractorCoreApi
    {

        private readonly List<ClassGeneration> classGenerations = new List<ClassGeneration>();

        public ContractorCoreApi()
        {
            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();
            classGenerations = serviceProvider.GetServices<ClassGeneration>().ToList();
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            if (!DomainAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                classGeneration.PerformAddDomainCommand(options);
            }
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            if (!EntityAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                classGeneration.PerformAddEntityCommand(options);
            }
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            if (!PropertyAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                classGeneration.PerformAddPropertyCommand(options);
            }
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            if (!RelationAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (ClassGeneration classGeneration in classGenerations)
            {
                classGeneration.PerformAdd1ToNRelationCommand(options);
            }
        }
    }
}