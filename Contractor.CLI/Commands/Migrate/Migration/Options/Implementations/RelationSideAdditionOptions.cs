﻿using Contractor.Core.Helpers;

namespace Contractor.CLI.Migration
{
    public class RelationSideAdditionOptions : EntityAdditionOptions, IRelationSideAdditionOptions
    {
        private string propertyName;

        public string PropertyType { get; set; }

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value.ToVariableName(); }
        }

        public bool IsOptional { get; set; }

        public RelationSideAdditionOptions()
        {
        }

        public RelationSideAdditionOptions(IContractorOptions options) : base(options)
        {
        }
    }
}