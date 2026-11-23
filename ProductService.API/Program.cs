using Infrastructure;
using ProductService.Application;
using ProductService.Grpc;
using ProductService.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();
app.AddInfrastructure();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService.API API"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.MapGrpcService<ProductGrpcService>();

app.Run();