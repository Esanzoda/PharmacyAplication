using Notification.Api_.Application.Services;
using Notification.Api_.Extensions;
using Notification.Api_.Infrastructure.Setting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(EmailOptions.SettingName));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddConsumers(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();