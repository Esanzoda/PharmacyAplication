using FluentValidation;
using PharmacyService.Contract.Request;

namespace GatewayService.Api.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is required");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Minimum Length 8 ");
    }
}