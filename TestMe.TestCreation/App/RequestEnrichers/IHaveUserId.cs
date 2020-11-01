using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.RequestEnrichers
{
    public interface IHaveUserId
    {
        long UserId { get; set; }
    }
}
