using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.UserManagement.App.Users.Input;
using TestMe.UserManagement.Domain;

namespace TestMe.Presentation.API.Controllers.Users.Input
{
    public class CreateUserDTO : IValidatableObject
    {
        [StringLength(maximumLength: User.NameMaxLength, MinimumLength = User.NameMinLength)]
        public string Name { get; set; } = String.Empty;

        [EmailAddress]
        [Required]
        [StringLength(maximumLength: TestMe.UserManagement.Domain.EmailAddress.MaxLength)]
        public string EmailAddress { get; set; } = String.Empty;

        [Required]
        [StringLength(maximumLength: TestMe.UserManagement.Domain.Password.MaxLength, MinimumLength = TestMe.UserManagement.Domain.Password.MinLength)]
        public string Password { get; set; } = String.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EmailAddress == Password)
            {
                yield return new ValidationResult(
                    "Nop, your password cannot be the same as email",
                    new[] { nameof(Password), nameof(EmailAddress) });
            }
        }


        public CreateUser CreateCommand()
        {
            return new CreateUser()
            {
                Name = Name,
                EmailAddress = EmailAddress,
                Password = Password
            };
        }
    }
}
