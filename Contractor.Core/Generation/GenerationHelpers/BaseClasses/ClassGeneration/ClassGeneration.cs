﻿using Contractor.Core.MetaModell;
using System;

namespace Contractor.Core.Generation
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

        protected virtual void PostGeneration(Entity entity) { }

        public void AddModule(Module module)
        {
            //try
            //{
            this.AddModuleActions(module);
            if (module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + module.Name);
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
                Console.WriteLine(this.GetType().Name + " completed successfully: " + entity.Name);
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
                Console.WriteLine(this.GetType().Name + " completed successfully: " + property.Entity.Name + "." + property.Name);
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
            if (relation.TargetEntity.Module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + relation.TargetEntity.Name + " -> " + relation.SourceEntity.Name);
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
            if (relation.TargetEntity.Module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + relation.TargetEntity.Name + " -> " + relation.SourceEntity.Name);
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
            if (relation.SourceEntity.Module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + relation.TargetEntity.Name + " -> " + relation.SourceEntity.Name);
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
            if (relation.SourceEntity.Module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + relation.TargetEntity.Name + " -> " + relation.SourceEntity.Name);
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

        public void PerformPostGenerationCommand(Entity entity)
        {
            //try
            //{
            this.PostGeneration(entity);
            if (entity.Module.Options.IsVerbose)
            {
                Console.WriteLine(this.GetType().Name + " completed successfully: " + entity.Name + " -> " + entity.Name);
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