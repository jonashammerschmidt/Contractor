﻿namespace Contractor.CLI
{
    internal class DomainHandler
    {
        public static void Perform(string[] args)
        {
            switch (args[0])
            {
                case "add":
                    DomainAdditionHandler.Perform(args);
                    break;
            }
        }
    }
}