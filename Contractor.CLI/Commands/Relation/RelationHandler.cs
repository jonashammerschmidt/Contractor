using System;

namespace Contractor.CLI
{
    public class RelationHandler
    {
        public static void Perform(string[] args)
        {
            switch (args[0])
            {
                case "add":
                    PerformAdd(args);
                    break;
            }
        }

        private static void PerformAdd(string[] args)
        {
            switch (args[2])
            {
                case "1:n":
                    Relation1ToNAdditionHandler.Perform(args);
                    break;

                default:
                    Console.WriteLine($"Relation type {args[2]} not found");
                    break;
            }
        }
    }
}