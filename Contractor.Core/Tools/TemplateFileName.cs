using Contractor.Core.Jobs;

namespace Contractor.Core.Tools
{
    internal class TemplateFileName
    {
        public static string GetFileNameForEntityAddition(IEntityAdditionOptions options, string originalTemplateFileName)
        {
            if (options.ForMandant)
            {
                originalTemplateFileName = originalTemplateFileName.Replace(".txt", "-Mandant.txt");
            }

            return originalTemplateFileName;
        }
    }
}