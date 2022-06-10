using Contractor.Core.Options;
using System;
using System.Collections.Generic;

namespace Contractor.Core.Projects
{
    public abstract class ClassGeneration
    {
        protected abstract void AddModuleActions(Module module);

        protected abstract void AddEntity(Entity entity);

        protected abstract void AddProperty(Property property);

        protected abstract void Add1ToNRelation(Relation1ToN relation);

        protected abstract void AddOneToOneRelation(Relation1To1 relation);

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

        public void PerformAddPropertyCommand(Property property)
        {
            try
            {
                this.AddProperty(property);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(this.GetType().Name + " completed successfully");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Property-Generierung: " + e.Message);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }

        public void PerformAdd1ToNRelationCommand(Relation1ToN relation)
        {
            try
            {
                this.Add1ToNRelation(new Relation1ToN(relation));
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(this.GetType().Name + " completed successfully");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }

        public void PerformAddOneToOneRelationCommand(Relation1To1 relation)
        {
            try
            {
                this.AddOneToOneRelation(relation);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(this.GetType().Name + " completed successfully");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
                //if (options.IsVerbose)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }
    }
}