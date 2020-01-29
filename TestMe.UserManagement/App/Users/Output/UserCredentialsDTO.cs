using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.App.Users.Output
{
    public class UserCredentialsDTO
    {
        public long Id { get; set; }
        public UserRole UserRole { get; set; }


        public UserCredentialsDTO(long id)
        {
            Id = id;
        }

        public UserCredentialsDTO(User user)
        {
            Id = user.UserId;
            UserRole = user.Role;
        }
    }
}
