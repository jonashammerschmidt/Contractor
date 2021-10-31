using Contractor.Core.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contractor.CLI
{
    internal static class ContractorDefaultOptionsFinder
    {
        internal static IContractorOptions FindDefaultOptions(string currentFolder)
        {
            string backendDestinationFolder = currentFolder;
            string dbDestinationFolder = FindBestDbDestinationFolder(currentFolder);
            string frontendDestinationFolder = FindBestFrontendDestinationFolder(currentFolder);
            string projectName = new DirectoryInfo(backendDestinationFolder).Name;
            string dbProjectName = new DirectoryInfo(dbDestinationFolder).Name;

            return new ContractorOptions()
            {
                BackendDestinationFolder = backendDestinationFolder,
                DbDestinationFolder = dbDestinationFolder,
                FrontendDestinationFolder = frontendDestinationFolder,
                ProjectName = projectName,
                Replacements = new Dictionary<string, string>(),
                DbProjectName = dbProjectName
            };
        }

        private static string FindBestDbDestinationFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);

            var path = FindDbDestinationFolder(di.FullName);
            if (path != null)
                return path;
            path = FindDbDestinationFolder(di.Parent.FullName);
            if (path != null)
                return path;
            path = FindDbDestinationFolder(di.Parent.Parent.FullName);
            if (path != null)
                return path;

            return null;
        }

        private static string FindDbDestinationFolder(string dir)
        {
            return Directory.GetDirectories(dir)
                .Concat(Directory.GetDirectories(dir)
                    .SelectMany(subDir => Directory.GetDirectories(subDir)))
                .Where(directory => new DirectoryInfo(directory).Name.EndsWith(".Database.Core"))
                .FirstOrDefault();
        }

        private static string FindBestFrontendDestinationFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);

            var path = FindFrontendDestinationFolder(di.FullName);
            if (path != null)
                return path;
            path = FindFrontendDestinationFolder(di.Parent.FullName);
            if (path != null)
                return path;
            path = FindFrontendDestinationFolder(di.Parent.Parent.FullName);
            if (path != null)
                return path;

            return null;
        }

        private static string FindFrontendDestinationFolder(string dir)
        {
            return Directory.GetDirectories(dir)
                .Concat(Directory.GetDirectories(dir)
                    .SelectMany(subDir => Directory.GetDirectories(subDir)))
                .Where(directory => new DirectoryInfo(directory).Name.EndsWith(".Web.Core"))
                .FirstOrDefault();
        }
    }
}