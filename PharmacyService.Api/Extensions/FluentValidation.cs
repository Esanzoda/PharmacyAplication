using FluentValidation;
using FluentValidation.AspNetCore;

namespace PharmacyService.Api.Extensions;

public static class FluentValidation
{
    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(FluentValidation).Assembly);
    }
}