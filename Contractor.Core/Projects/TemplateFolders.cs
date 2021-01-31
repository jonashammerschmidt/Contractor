namespace Contractor.Core.Template
{
    internal class TemplateFolders
    {
        public static string[] Folders = new string[] {
            "API",
            "Contracts",
            "Logic",
            "Persistence"
        };

        public static string[] DomainFolders = new string[] {
            "API/Model/{Domain}",
            "Contract/Logic/Model/{Domain}",
            "Contract/Persistence/Model/{Domain}",
            "Logic/Model/{Domain}",
            "Persistence/Model/{Domain}"
        };
    }
}