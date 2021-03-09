using Contractor.Core.Options;
using System;

namespace Contractor.Core.Projects
{
    public abstract class ClassGeneration
    {
        protected abstract void AddDomain(IDomainAdditionOptions options);

        protected abstract void AddEntity(IEntityAdditionOptions options);

        protected abstract void AddProperty(IPropertyAdditionOptions options);

        protected abstract void Add1ToNRelation(IRelationAdditionOptions options);

        public void PerformAddDomainCommand(IDomainAdditionOptions options)
        {
            try
            {
                this.AddDomain(options);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Domain-Generierung: " + e.Message);
            }
        }

        public void PerformAddEntityCommand(IEntityAdditionOptions options)
        {
            try
            {
                this.AddEntity(options);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Entity-Generierung: " + e.Message);
            }
        }

        public void PerformAddPropertyCommand(IPropertyAdditionOptions options)
        {
            try
            {
                this.AddProperty(options);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Property-Generierung: " + e.Message);
            }
        }
        public void PerformAdd1ToNRelationCommand(IRelationAdditionOptions options)
        {
            try
            {
                this.Add1ToNRelation(options);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
            }
        }
    }
}