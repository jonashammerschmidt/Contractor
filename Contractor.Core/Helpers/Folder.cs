using System.IO;

namespace Contractor.Core.Helpers
{
    public class Folder
    {
        public static string Executable = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).DirectoryName;
    }
}