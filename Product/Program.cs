using Product.Infrastructure.Extensions;
using Product.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddProductDbContext(builder.Configuration);
builder.Services.ADDRedis(builder.Configuration);
builder.Services.AddFluentValidation();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatr();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();