using ProductService.Api.Infrastructure.Extensions;
using ProductService.Api.Middlewares;
using ProductService.Api.Services.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR();
builder.Services.AddFluentValidation();
builder.Services.AddProductDbContext(builder.Configuration);
builder.Services.AddGrpcClient();
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddMasstransit(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddRedis(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService API"); });
}

app.AddJob();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.MapGrpcService<ProductGrpcService>();

app.Run();