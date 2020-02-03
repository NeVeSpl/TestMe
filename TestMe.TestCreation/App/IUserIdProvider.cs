using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App
{
    public interface IUserIdProvider
    {
        public long UserId
        {
            get;
        }
    }
}
