using System;
using System.ComponentModel.DataAnnotations;
using TestMe.UserManagement.App.Users.Input;
using TestMe.UserManagement.Domain;

namespace TestMe.Presentation.API.Controllers.Tokens.Input
{
    public class LoginCredentialsDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(maximumLength: TestMe.UserManagement.Domain.EmailAddress.MaxLength)]
        public string Email { get; set; } = String.Empty;

        [Required]
        [StringLength(maximumLength: TestMe.UserManagement.Domain.Password.MaxLength, MinimumLength = TestMe.UserManagement.Domain.Password.MinLength)]
        public string Password { get; set; } = String.Empty;



        public LoginUser CreateCommand()
        {
            return new LoginUser()
            {
                Email = Email,
                Password = Password
            };
        }
    }
}
