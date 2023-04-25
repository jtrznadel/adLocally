using FluentValidation;
using ST2Project.Logic.Entities;
using ST2Project.Logic.Services;
using ST2Project.UI.Models;

namespace ST2Project.UI.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).Length(1, 10);
            RuleFor(x => x.LastName).Length(1, 15);
            RuleFor(x => x.Username).Length(1, 15)
            .Must(IsUsernameUnique)
            .WithMessage("Username already exists"); ;
            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .Must(IsEmailUnique)
            .WithMessage("Email already exists");
            RuleFor(x => x.Password).Length(1, 15);
            RuleFor(x => x.ConfirmPassword).Length(1, 15).Equal(x => x.Password).WithMessage("Passwords do not match");         
        }

        public bool IsEmailUnique(string value)
        {
            return !new UserService().IsEmailExists(value); ;
        }

        public bool IsUsernameUnique(string value)
        {
            return !new UserService().IsUsernameExists(value); ;
        }
    }
}
