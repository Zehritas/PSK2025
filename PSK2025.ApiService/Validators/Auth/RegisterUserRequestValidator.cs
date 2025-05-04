using PSK2025.Data.Requests.Auth;
using FluentValidation;

namespace PSK2025.ApiService.Validators.Auth;

public class RegisterUserRequestValidator : AbstractValidator<RegisterNewUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");
    }
}
