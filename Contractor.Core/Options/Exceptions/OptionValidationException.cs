using System;

namespace Contractor.Core.Options
{
    public class OptionValidationException : ApplicationException
    {
        public OptionValidationException(string message) : base(message)
        {
        }
    }
}