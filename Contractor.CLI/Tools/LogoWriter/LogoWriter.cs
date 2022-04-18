using System;

namespace Contractor.CLI.Tools
{
    internal static class LogoWriter
    {
        private static readonly string Logo = string.Join(
            Environment.NewLine,
            @"_________               _____                        _____",
            @"__  ____/______ _______ __  /_______________ __________  /_______ ________",
            @"_  /     _  __ \__  __ \_  __/__  ___/_  __ `/_  ___/_  __/_  __ \__  ___/",
            @"/ /___   / /_/ /_  / / // /_  _  /    / /_/ / / /__  / /_  / /_/ /_  /",
            @"\____/   \____/ /_/ /_/ \__/  /_/     \__,_/  \___/  \__/  \____/ /_/");

        public static void Write()
        {
            ConsoleWriteLineColor(Logo);
            Console.WriteLine();
            Console.WriteLine("Version: " + GetAppVersion());
        }

        private static void ConsoleWriteLineColor(string text)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.WriteLine(text);

            Console.ForegroundColor = consoleColor;
        }

        private static string GetAppVersion()
        {
            return typeof(LogoWriter).Assembly.GetName().Version.ToString();
        }
    }
}