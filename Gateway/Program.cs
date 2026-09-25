using Gateway.Infrastructure.Extensions;
using Gateway.Middlewares;
using Pharmacy.Endpoints.NuGet.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwagger();
builder.Services.AddControllers();
builder.Services.AddPharmacyApi();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "Gateway Api"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();