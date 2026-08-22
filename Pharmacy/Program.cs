using Hangfire;
using Pharmacy.Infrastructure.Extensions;
using Pharmacy.Middlewares;
using Pharmacy.Infrastructure.Setting;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionStringsOption>(
    builder.Configuration.GetSection(ConnectionStringsOption.SettingName));
builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection(JwtOption.SettingName));

builder.Services.Configure<GoogleMap>(
    builder.Configuration.GetSection(GoogleMap.SettingName));

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.AddSeriaLogger();
builder.Services.AddInfrastructure();
builder.Services.AddSwagger();
builder.Services.AddMassTransit(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(
                "http://localhost:5500",
                "http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();
app.AddJob();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy API"); });
}

app.UseHangfireDashboard();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Frontend"); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();