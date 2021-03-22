using System.IO;

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