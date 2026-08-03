using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Auth.Commands;

public record LoginCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginHandler(
    IMediator mediator,
    IApplicationDbContext dbContext,
    IPasswordService passwordService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email, cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException("Customer not found");
        }

        var passwordCheck = await passwordService.PasswordVerify(request.Request.Password, customer.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accessToken = await mediator.Send(new GenerateTokenCommand(customer), cancellationToken);
        var newRefreshToken =
            new RefreshToken
            {
                CustomerId = customer.Id,
                Token = await mediator.Send(new GenerateRefreshTokenCommand(), cancellationToken),
                ExpiresAt = now
                    .AddDays(7),
                CreatedAt = now
            };
        await dbContext.RefreshTokens
            .AddAsync(newRefreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}