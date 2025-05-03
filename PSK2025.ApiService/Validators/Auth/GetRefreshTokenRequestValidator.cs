using FluentValidation;
using PSK2025.Data.Requests.Auth;

namespace PSK2025.ApiService.Validators.Auth;

public class GetRefreshTokenRequestValidator : AbstractValidator<GetRefreshTokenRequest>
{
    public GetRefreshTokenRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required");
    }
}
