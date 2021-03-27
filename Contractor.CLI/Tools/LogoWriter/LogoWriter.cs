using Colorful;
using Contractor.Core.Helpers;
using System.Drawing;
using System.IO;

namespace Contractor.CLI.Tools
{
    internal static class LogoWriter
    {
        public static void Write()
        {
            string FontPath = Path.Combine(Folder.Executable, "Tools", "LogoWriter", "speed.flf");
            Console.WriteAscii("Contractor", FigletFont.Load(FontPath), Color.FromArgb(88, 116, 255));
            System.Console.WriteLine("Version: " + GetAppVersion());
        }

        private static string GetAppVersion()
        {
            return typeof(LogoWriter).Assembly.GetName().Version.ToString();
        }
    }
}