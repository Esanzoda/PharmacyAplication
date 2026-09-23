using Microsoft.OpenApi;

namespace Gateway.Infrastructure.Extensions;

public static class Swagger
{
    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("V1", new OpenApiInfo
            {
                Title = "Gateway Api", Version = "V1"
            });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT Token (dont need 'Bearer')"
            });
            option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document),
                    new List<string>()
                }
            });
        });
    }
}