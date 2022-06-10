﻿using Contractor.Core;
using Contractor.Core.Helpers;
using System.Collections.Generic;

namespace Contractor.Core
{
    public class Module
    {
        public IEnumerable<Entity> Entities { get; set; }

        public string Name { get; set; }

        public string NameKebab
        {
            get { return StringConverter.PascalToKebabCase(Name); }
        }

        public bool Skip { get; set; }

        public ContractorGenerationOptions Options { get; private set; }

        public void AddLinks(ContractorGenerationOptions options)
        {
            Options = options;

            foreach (var entity in Entities)
            {
                entity.AddLinks(this);
            }
        }
    }
}