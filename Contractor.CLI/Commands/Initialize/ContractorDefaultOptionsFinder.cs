using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contractor.CLI
{
    internal static class ContractorDefaultOptionsFinder
    {
        internal static ContractorXml FindDefaultOptions(string currentFolder)
        {
            string bestDbDestinationFolder = FindBestDbDestinationFolder(currentFolder);
            return new ContractorXml()
            {
                Paths = new PathsXml()
                {
                    BackendDestinationFolder = currentFolder,
                    DbDestinationFolder = bestDbDestinationFolder,
                    FrontendDestinationFolder = FindBestFrontendDestinationFolder(currentFolder),
                    ProjectName = new DirectoryInfo(currentFolder).Name,
                    GeneratedProjectName = new DirectoryInfo(currentFolder).Name,
                    DbProjectName = new DirectoryInfo(bestDbDestinationFolder).Name,
                    DbContextName = "FullstackTemplateCoreDbContext"
                },
                Replacements = new ReplacementsXml()
                {
                    Replacements = new List<ReplacementXml>(),
                },
                Modules = new ModulesXml()
                {
                    Modules = new List<ModuleXml>(),
                }
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