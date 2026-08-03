using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.CQRS.Deliver.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Deliver.Commands;

public record CreateDeliverCommand(
    DeliverRequest Request) : IRequest<DeliverResponse>;

public class CreateDeliverCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(CreateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliverExists = await dbContext.Delivers
            .AnyAsync(x => x.Email == request.Request.Email ||
                           x.PhoneNumber == request.Request.PhoneNumber, cancellationToken);

        if (deliverExists)
        {
            throw new RecourseIsAlreadyExistException("Deliver already exists");
        }

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);
        var newDeliver = mapper.Map<Models.Deliver>(request.Request);
        newDeliver.PasswordHash = passwordHash;
        await dbContext.Delivers
            .AddAsync(newDeliver, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DeliverResponse>(newDeliver);
    }
}