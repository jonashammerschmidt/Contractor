using System;

namespace Contractor.CLI.Migration
{
    public class OptionValidationException : ApplicationException
    {
        public OptionValidationException(string message) : base(message)
        {
        }
    }
}