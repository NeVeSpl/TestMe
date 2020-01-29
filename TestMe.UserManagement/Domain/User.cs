using TestMe.SharedKernel.Domain;
using TestMe.UserManagement.IntegrationEvents;

namespace TestMe.UserManagement.Domain
{
    public sealed class User
    {
        public const int NameMaxLength = 256;
        public const int NameMinLength = 3;

        public long UserId { get; set; }
        //public Guid UserId { get; set; }        
        public string? Name { get; set; }
        public EmailAddress EmailAddress { get; set; }
        public Password Password { get; set; }
        public UserRole Role { get; set; }
        public MembershipLevel MembershipLevel { get; set; }



#pragma warning disable CS8618 // Non-nullable field is uninitialized. 
        private User()
        {
            // We need this constructor because ef core 3.1 is not able to use constructor with owned types as parameters
        }
#pragma warning restore CS8618
       

        public User(EmailAddress emailAddress, Password password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }




        public UserCreatedV1 CreateEvent()
        {
            return new UserCreatedV1()
            {
                UserId = UserId,
                MembershipLevel = MembershipLevel
            };
        }
    }
}