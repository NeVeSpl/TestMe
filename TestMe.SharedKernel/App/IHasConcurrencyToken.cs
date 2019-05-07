using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.SharedKernel.App
{
    public interface IHasConcurrencyToken
    {
        uint ConcurrencyToken { get; }
    }
}
