using System.IO;
using System.Text;

namespace Contractor.Core.Tools
{
    internal class TypescriptClassWriter
    {
        public static void Write(string filePath, string fileData)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, fileData);
        }
    }
}