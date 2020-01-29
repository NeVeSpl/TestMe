using TestMe.SharedKernel.Domain;

namespace TestMe.UserManagement.IntegrationEvents
{
    public sealed class UserCreatedV1
    {
        public long UserId { get; set; }    
        public MembershipLevel MembershipLevel { get; set; }

        public UserCreatedV1()
        {
           
        }
    }
}
