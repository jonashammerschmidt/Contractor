﻿using Contractor.Core.MetaModell;

namespace Contractor.Core.Tools
{
    internal class TemplateFileName
    {
        public static string GetFileNameForEntityAddition(Entity entity, string originalTemplateFileName)
        {
            if (entity.HasScope)
            {
                originalTemplateFileName = originalTemplateFileName.Replace(".txt", "-RequestScope.txt");
            }

            return originalTemplateFileName;
        }
    }
}