using Contractor.Core.Options;
using System;
using System.Collections.Generic;

namespace Contractor.Core.Projects
{
    public abstract class ClassGeneration
    {
        protected abstract void AddModuleActions(Module module);

        protected abstract void AddEntity(Entity entity);

        protected abstract void AddProperty(IPropertyAdditionOptions options);

        protected abstract void Add1ToNRelation(IRelationAdditionOptions options);

        protected abstract void AddOneToOneRelation(IRelationAdditionOptions options);

        public void AddModule(Module module)
        {
            try
            {
                this.AddModuleActions(module);
                //if (module.IsVerbose)
                //{
                //    Console.WriteLine(this.GetType().Name + " completed successfully");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Domain-Generierung: " + e.Message);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }

        public void PerformAddEntityCommand(Entity entity)
        {
            try
            {
                this.AddEntity(entity);
                //if (entity.IsVerbose)
                //{
                //    Console.WriteLine(this.GetType().Name + " completed successfully");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Entity-Generierung: " + e.Message);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }

        public void PerformAddPropertyCommand(IPropertyAdditionOptions options)
        {
            try
            {
                this.AddProperty(options);
                if (options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Property-Generierung: " + e.Message);
                if (options.IsVerbose)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public void PerformAdd1ToNRelationCommand(IRelationAdditionOptions options)
        {
            try
            {
                this.Add1ToNRelation(options);
                if (options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
                if (options.IsVerbose)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public void PerformAddOneToOneRelationCommand(IRelationAdditionOptions options)
        {
            try
            {
                this.AddOneToOneRelation(options);
                if (options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
                if (options.IsVerbose)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}