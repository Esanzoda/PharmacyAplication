using Notifications.Infrastructure.Extensions;
using Notifications.Services;
using Notifications.Setting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(EmailOptions.SettingName));

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddConsumers(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();