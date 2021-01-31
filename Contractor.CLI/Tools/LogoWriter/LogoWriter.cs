using Colorful;
using Contractor.Core.Helpers;
using System.Drawing;
using System.IO;

namespace Contractor.CLI.Tools
{
    public static class LogoWriter
    {
        public static void Write()
        {
            string FontPath = Path.Combine(Folder.Executable, "Tools", "LogoWriter", "speed.flf");
            Console.WriteAscii("Contractor", FigletFont.Load(FontPath), Color.FromArgb(88, 116, 255));
        }
    }
}