namespace Contractor.CLI
{
    internal class EntityHandler
    {
        public static void Perform(string[] args)
        {
            switch (args[0])
            {
                case "add":
                    EntityAdditionHandler.Perform(args);
                    break;
            }
        }
    }
}