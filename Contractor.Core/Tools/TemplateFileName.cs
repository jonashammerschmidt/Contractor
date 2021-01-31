using Contractor.Core.Jobs.EntityAddition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contractor.Core.Tools
{
    class TemplateFileName
    {
        public static string GetFileNameForEntityAddition(EntityOptions options, string originalTemplateFileName)
        {
            if (options.ForMandant)
            {
                originalTemplateFileName = originalTemplateFileName.Replace(".txt", "-Mandant.txt");
            }

            return originalTemplateFileName;
        }
    }
}
