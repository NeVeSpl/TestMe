using System;

namespace TestMe.SharedKernel.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception innerException = null)
            : base(message, innerException)
        {            
        }
    }
}
