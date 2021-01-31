using Newtonsoft.Json;
using Contractor.Core.Jobs;
using System.IO;

namespace Contractor.CLI
{
    public class ContractorOptionsLoader
    {
        public static IContractorOptions Load(string folder) {

            string optionsPath = Path.Combine(folder, "contractor.json");
            string optionsJson = File.ReadAllText(optionsPath);
            ContractorOptions options = JsonConvert.DeserializeObject<ContractorOptions>(optionsJson);
            options.BackendDestinationFolder = Path.Combine(folder, options.BackendDestinationFolder);
            options.DbDestinationFolder = Path.Combine(folder, options.DbDestinationFolder);
            return options;
        }
    }
}