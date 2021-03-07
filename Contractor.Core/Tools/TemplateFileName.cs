using Contractor.Core.Options;

namespace Contractor.Core.Tools
{
    internal class TemplateFileName
    {
        public static string GetFileNameForEntityAddition(IEntityAdditionOptions options, string originalTemplateFileName)
        {
            if (options.HasRequestScope)
            {
                originalTemplateFileName = originalTemplateFileName.Replace(".txt", "-RequestScope.txt");
            }

            return originalTemplateFileName;
        }
    }
}