using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Deliver.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Api.Services.Password;

namespace PharmacyService.Api.Application.CQRS.Deliver.Commands;

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