using System;
using TestMe.SharedKernel.Domain;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.App.Users.Output
{
    public class UserDTO
    {
        public long UserId { get; set; }            
        public string? Name { get; set; }
        public string EmailAddress { get; set; } = String.Empty;

        public UserRole Role { get; set; }
        public MembershipLevel MembershipLevel { get; set; }
    }
}
