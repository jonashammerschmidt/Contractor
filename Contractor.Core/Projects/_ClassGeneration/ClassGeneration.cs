using System;

namespace Contractor.Core.Projects
{
    public abstract class ClassGeneration
    {
        protected abstract void AddModuleActions(Module module);

        protected abstract void AddEntity(Entity entity);

        protected abstract void AddProperty(Property property);

        protected abstract void Add1ToNRelationSideFrom(Relation1ToN relation);

        protected abstract void AddOneToOneRelationSideFrom(Relation1To1 relation);

        protected abstract void Add1ToNRelationSideTo(Relation1ToN relation);

        protected abstract void AddOneToOneRelationSideTo(Relation1To1 relation);

        public void AddModule(Module module)
        {
            //try
            //{
                this.AddModuleActions(module);
                if (module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Domain-Generierung: " + e.Message);
            //    if (module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAddEntityCommand(Entity entity)
        {
            //try
            //{
                this.AddEntity(entity);
                if (entity.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Entity-Generierung: " + e.Message);
            //    if (entity.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAddPropertyCommand(Property property)
        {
            //try
            //{
                this.AddProperty(property);
                if (property.Entity.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Property-Generierung: " + e.Message);
            //    if (property.Entity.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAdd1ToNRelationSideFromCommand(Relation1ToN relation)
        {
            //try
            //{
                this.Add1ToNRelationSideFrom(new Relation1ToN(relation));
                if (relation.EntityFrom.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
            //    if (relation.EntityFrom.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAddOneToOneRelationSideFromCommand(Relation1To1 relation)
        {
            //try
            //{
                this.AddOneToOneRelationSideFrom(relation);
                if (relation.EntityFrom.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
            //    if (relation.EntityFrom.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAdd1ToNRelationSideToCommand(Relation1ToN relation)
        {
            //try
            //{
                this.Add1ToNRelationSideTo(new Relation1ToN(relation));
                if (relation.EntityTo.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
            //    if (relation.EntityTo.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }

        public void PerformAddOneToOneRelationSideToCommand(Relation1To1 relation)
        {
            //try
            //{
                this.AddOneToOneRelationSideTo(relation);
                if (relation.EntityTo.Module.Options.IsVerbose)
                {
                    Console.WriteLine(this.GetType().Name + " completed successfully");
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Fehler bei Relation-Generierung: " + e.Message);
            //    if (relation.EntityTo.Module.Options.IsVerbose)
            //    {
            //        Console.WriteLine(e.StackTrace);
            //    }
            //}
        }
    }
}