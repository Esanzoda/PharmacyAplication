using ProductService.Infrastructure.Extensions;
using ProductService.Jobs;
using ProductService.Middlewares;
using ProductService.Service.Grpc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddProductDbContext(builder.Configuration);
builder.Services.ADDRedis(builder.Configuration);
builder.Services.AddFluentValidation();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatr();
builder.Services.AddGrpc();
builder.Services.AddGrpcClient();
builder.Services.AddScoped<CheckExpiredProductsJob>();
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddMasstransit(builder.Configuration);

var app = builder.Build();
app.AddJob();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService API"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.MapGrpcService<ProductGrpcService>();

app.Run();