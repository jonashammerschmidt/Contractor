using System.IO;

namespace Contractor.Core.Tools
{
    internal class CsharpClassWriter
    {
        public static void Write(string filePath, string fileData)
        {
            fileData = UsingStatements.Sort(fileData);

            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, fileData);
        }
    }
}