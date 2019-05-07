using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.UserManagement.Domain
{
    public class User
    {
        public const int NameMaxLength = 256;
        public const int NameMinLength = 3;

        public long UserId { get; set; }
        public string Name { get; set; }
    }
}
