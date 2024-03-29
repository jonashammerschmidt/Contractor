﻿using System;

namespace Contractor.Core.Generation
{
    public sealed class ClassGenerationTagsAttribute : Attribute
    {
        private ClassGenerationTag[] tags;

        public ClassGenerationTagsAttribute(ClassGenerationTag[] tags)
        {
            this.tags = tags;
        }

        public ClassGenerationTag[] GetGenerationTags()
        {
            return tags;
        }
    }
}