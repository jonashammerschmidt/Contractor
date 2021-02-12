using System;

namespace Contractor.Core
{
    public class OptionValidationException : ApplicationException
    {
        public OptionValidationException(string message) : base(message)
        {
        }
    }
}