using Contractor.Core.Options;
using Contractor.Core.Projects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class ContractorCoreApi
    {

        private List<IProjectGeneration> projectGenerations = new List<IProjectGeneration>();

        public ContractorCoreApi()
        {
            ServiceProvider serviceProvider = DependencyProvider.GetServiceProvider();
            projectGenerations.Add(serviceProvider.GetService<ContractPersistenceProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<ContractLogicProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<PersistenceProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<PersistenceTestsProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<LogicProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<LogicTestsProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<ApiProjectGeneration>());
            projectGenerations.Add(serviceProvider.GetService<DBProjectGeneration>());
        }

        public void AddDomain(IDomainAdditionOptions options)
        {
            if (!DomainAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (IProjectGeneration projectGeneration in projectGenerations)
            {
                projectGeneration.AddDomain(options);
            }
        }

        public void AddEntity(IEntityAdditionOptions options)
        {
            if (!EntityAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (IProjectGeneration projectGeneration in projectGenerations)
            {
                projectGeneration.AddEntity(options);
            }
        }

        public void AddProperty(IPropertyAdditionOptions options)
        {
            if (!PropertyAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (IProjectGeneration projectGeneration in projectGenerations)
            {
                projectGeneration.AddProperty(options);
            }
        }

        public void Add1ToNRelation(IRelationAdditionOptions options)
        {
            if (!RelationAdditionOptions.Validate(options))
            {
                throw new OptionValidationException("Die Optionen sind nicht korrekt formatiert.");
            }

            foreach (IProjectGeneration projectGeneration in projectGenerations)
            {
                projectGeneration.Add1ToNRelation(options);
            }
        }
    }
}