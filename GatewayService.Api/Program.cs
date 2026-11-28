using GatewayService.Api.Infrastructure.Extensions;
using GatewayService.Api.Middlewares;
using PharmacyService.ApiClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(option =>
{
    option.CustomSchemaIds(type => type.FullName );
});
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddFluentValidation();
builder.Services.AddPharmacyApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewayService.Api"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();