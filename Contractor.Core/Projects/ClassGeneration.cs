using Contractor.Core.Options;
using System;

namespace Contractor.Core.Projects
{
    public abstract class ClassGeneration
    {
        protected abstract void Add1ToNRelation(IRelationAdditionOptions options);

        protected abstract void AddDomain(IDomainAdditionOptions options);

        protected abstract void AddEntity(IEntityAdditionOptions options);

        protected abstract void AddProperty(IPropertyAdditionOptions options);

        public void PerformAdd1ToNRelationCommand(IRelationAdditionOptions options)
        {
            try
            {
                this.Add1ToNRelation(options);
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }

        public void PerformAddDomainCommand(IDomainAdditionOptions options)
        {
            try
            {
                this.AddDomain(options);
            }
            catch (Exception e)
            {
                this.HandleException(e);
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
                this.HandleException(e);
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
                this.HandleException(e);
            }
        }

        private void HandleException(Exception e)
        {
            Console.WriteLine("Fehler bei Generierung: " + e.Message);
        }
    }
}