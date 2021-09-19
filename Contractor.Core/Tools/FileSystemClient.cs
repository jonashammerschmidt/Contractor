using System.IO;

namespace Contractor.Core.Tools
{
    internal class FileSystemClient
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            if (path.EndsWith(".cs")) {
                contents = UsingStatements.Sort(contents);
            }

            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(path, contents);
        }
    }
}