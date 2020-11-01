using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.Ports
{
    public interface IUserIdProvider
    {
        public long UserId
        {
            get;
        }
    }
}
