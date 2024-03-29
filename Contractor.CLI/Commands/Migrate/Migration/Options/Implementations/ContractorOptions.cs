﻿using System.Collections.Generic;

namespace Contractor.CLI.Migration
{
    public class ContractorOptions : IContractorOptions
    {
        public string BackendDestinationFolder { get; set; }

        public string DbDestinationFolder { get; set; }

        public string FrontendDestinationFolder { get; set; }

        public string ProjectName { get; set; }

        public string GeneratedProjectName { get; set; }

        public string DbProjectName { get; set; }

        public Dictionary<string, string> Replacements { get; set; }

        public bool IsVerbose { get; set; }

        public ContractorOptions()
        {
            this.Replacements = new Dictionary<string, string>();
        }

        public ContractorOptions(IContractorOptions options)
        {
            this.FrontendDestinationFolder = options.FrontendDestinationFolder;
            this.BackendDestinationFolder = options.BackendDestinationFolder;
            this.DbDestinationFolder = options.DbDestinationFolder;
            this.ProjectName = options.ProjectName;
            this.GeneratedProjectName = options.GeneratedProjectName;
            this.DbProjectName = options.DbProjectName;
            this.Replacements = options.Replacements;
            this.IsVerbose = options.IsVerbose;
        }
    }
}