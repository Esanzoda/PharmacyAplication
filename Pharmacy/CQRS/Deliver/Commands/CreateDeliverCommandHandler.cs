using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.Domain.Models.Deliver.DTOs.Request;
using Pharmacy.Domain.Models.Deliver.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Deliver.Commands;

public record CreateDeliverCommand(
    DeliverRequest Request) : IRequest<DeliverResponse>;

public class CreateDeliverCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(
        CreateDeliverCommand request,
        CancellationToken cancellationToken)
    {
        var deliverExists = await dbContext.Delivers
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber, cancellationToken);

        if (deliverExists)
        {
            throw new ResourceIsAlreadyExistException("Deliver already exists");
        }

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);

        var newDeliver = DeliverMappers.ToDeliver(request.Request);
        newDeliver.PasswordHash = passwordHash;

        await dbContext.Delivers
            .AddAsync(newDeliver, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return DeliverMappers.ToDeliverResponse(newDeliver);
    }
}