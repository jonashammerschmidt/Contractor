using System.IO;

namespace Contractor.Core.Tools
{
    internal class CsharpClassWriter
    {
        public static void Write(string filePath, string fileData)
        {
            fileData = UsingStatements.Sort(fileData);
            File.WriteAllText(filePath, fileData);
        }
    }
}