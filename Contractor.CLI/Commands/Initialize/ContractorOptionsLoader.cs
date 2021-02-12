using Contractor.Core.Options;
using Newtonsoft.Json;
using System.IO;

namespace Contractor.CLI
{
    internal class ContractorOptionsLoader
    {
        public static IContractorOptions Load(string folder)
        {
            string optionsPath = Path.Combine(folder, "contractor.json");
            string optionsJson = File.ReadAllText(optionsPath);
            ContractorOptions options = JsonConvert.DeserializeObject<ContractorOptions>(optionsJson);
            options.BackendDestinationFolder = Path.Combine(folder, options.BackendDestinationFolder);
            options.DbDestinationFolder = Path.Combine(folder, options.DbDestinationFolder);
            return options;
        }
    }
}