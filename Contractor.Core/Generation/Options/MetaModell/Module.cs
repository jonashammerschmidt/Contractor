﻿using Contractor.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Contractor.Core.MetaModell
{
    public class Module
    {
        private string name;

        public List<Entity> Entities { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value.ToVariableName(); }
        }

        public string NameKebab
        {
            get { return Name.PascalToKebabCase(); }
        }

        public string NameReadable
        {
            get { return Name.ToReadable(); }
        }

        public bool Skip { get; set; }

        public GenerationOptions Options { get; private set; }

        public void AddLinks(GenerationOptions options)
        {
            Options = options;
        }

        public void AddLinksForChildren()
        {
            foreach (var entity in Entities)
            {
                entity.AddLinks(this);
            }

            foreach (var entity in Entities)
            {
                entity.AddLinksForChildren();
            }
        }

        public void Sort(IEnumerable<Entity> sortedEntities)
        {
            Entities = sortedEntities
                .Where(entity => entity.Module == this)
                .ToList();
        }
    }
}