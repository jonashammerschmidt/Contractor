﻿using System.Collections.Generic;

namespace Contractor.CLI.Migration
{
    public interface IContractorOptions
    {
        string BackendDestinationFolder { get; set; }

        string DbDestinationFolder { get; set; }

        string FrontendDestinationFolder { get; set; }

        string ProjectName { get; set; }

        string GeneratedProjectName { get; set; }

        string DbProjectName { get; set; }

        Dictionary<string, string> Replacements { get; set; }

        bool IsVerbose { get; set; }
    }
}