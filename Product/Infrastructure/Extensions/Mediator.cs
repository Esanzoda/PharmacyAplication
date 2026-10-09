namespace Product.Infrastructure.Extensions;

public static class Mediator
{
    public static void AddMediatr(this IServiceCollection service)
    {
        service.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
    }
}