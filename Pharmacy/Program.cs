using Hangfire;
using Pharmacy.Infrastructure.Extensions;
using Pharmacy.Middlewares;
using Pharmacy.Infrastructure.Setting;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionStringsOption>(
    builder.Configuration.GetSection(ConnectionStringsOption.SettingName));
builder.Services.Configure<RabbitMqOption>(
    builder.Configuration.GetSection(RabbitMqOption.SettingName));
builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection(JwtOption.SettingName));
builder.Services.Configure<EmailOption>(
    builder.Configuration.GetSection(EmailOption.SettingName));
builder.Services.Configure<GoogleMap>(
    builder.Configuration.GetSection(GoogleMap.SettingName));

builder.Services.AddConsumers(builder.Configuration);
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.AddSeriaLogger();
builder.Services.AddInfrastructure();
builder.Services.AddSwagger();


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();