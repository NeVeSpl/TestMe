using System.ComponentModel.DataAnnotations;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.App.Users.DTO
{
    public class LoginCredentials
    {
        [Required]
        [StringLength(maximumLength: User.NameMaxLength, MinimumLength = User.NameMinLength)]
        public string Login { get; set; }

        [Required]
        [StringLength(maximumLength: 256, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
