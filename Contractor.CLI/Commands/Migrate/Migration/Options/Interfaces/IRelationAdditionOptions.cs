﻿namespace Contractor.CLI.Migration
{
    public interface IRelationAdditionOptions : IContractorOptions
    {
        bool IsOptional { get; set; }

        // From
        string DomainFrom { get; set; }

        string EntityNameFrom { get; set; }

        string EntityNameLowerFrom { get; }

        string EntityNamePluralFrom { get; set; }

        string EntityNamePluralLowerFrom { get; }

        string PropertyNameFrom { get; set; }

        // To
        string DomainTo { get; set; }

        string EntityNameLowerTo { get; }

        string EntityNameTo { get; set; }

        string EntityNamePluralLowerTo { get; }

        string EntityNamePluralTo { get; set; }

        string PropertyNameTo { get; set; }
    }
}