using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.GoogleMaps;

namespace Pharmacy.CQRS.Customer.Commands;

public record UpdateCustomerAddressCommand(
    long CustomerId,
    string NewAddress) : IRequest<string>;

public class UpdateCustomerAddressCommandHandler(
    IApplicationDbContext dbContext,
    IGeocodingService geocodingService) : IRequestHandler<UpdateCustomerAddressCommand, string>
{
    public async Task<string> Handle(
        UpdateCustomerAddressCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FindAsync([request.CustomerId],
                cancellationToken);
        if (customer is null)
        {
            throw new ResourceNotFoundException("Customer not found");
        }

        var geocoding = await geocodingService.GetCoordinatesAsync(request.NewAddress);
        customer.Address = request.NewAddress;
        customer.Latitude = geocoding.Lat;
        customer.Longitude = geocoding.Lng;

        await dbContext.SaveChangesAsync(cancellationToken);
        return $"Your address successful updated to {customer.Address}";
    }
}