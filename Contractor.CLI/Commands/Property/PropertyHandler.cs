namespace Contractor.CLI
{
    internal class PropertyHandler
    {
        public static void Perform(string[] args)
        {
            switch (args[0])
            {
                case "add":
                    PropertyAdditionHandler.Perform(args);
                    break;
            }
        }
    }
}