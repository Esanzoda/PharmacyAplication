using System.Security.Cryptography;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Auth.Commands;

public record ForgotCommand(
    string Email,
    Role Role) : IRequest<string>;

public class ForgotCommandHandler(
    IApplicationDbContext dbContext,
    IPublishEndpoint publishEndpoint,
    IDistributedCache cache) : IRequestHandler<ForgotCommand, string>
{
    public async Task<string> Handle(ForgotCommand request, CancellationToken cancellationToken)
    {
        var user = dbContext.Users
            .FirstOrDefault(x => x.Role == request.Role &&
                                 x.Email == request.Email);
        if (user is null)
        {
            throw new ResourceNotFoundException("User not found");
        }

        var newCode = RandomNumberGenerator.GetInt32(12345678);
        var key = $"User-{user.Email}-{user.Role}";
        await cache.SetStringAsync(
            key, newCode.ToString(),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, cancellationToken);
        await publishEndpoint.Publish(new ForgotPasswordEvent
            {
                To = user.Email,
                Message = newCode.ToString()
            },
            cancellationToken);
        var response = $"Your password successful updated chek your email {user.Email}";

        return response;
    }
}