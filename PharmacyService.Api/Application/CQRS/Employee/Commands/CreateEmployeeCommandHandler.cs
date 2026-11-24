using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Employee.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Api.Services.GoogleMaps;
using PharmacyService.Api.Services.Password;

namespace PharmacyService.Api.Application.CQRS.Employee.Commands;

public record CreateEmployeeCommand(
    long PharmacyId,
    EmployeeRequest Request) : IRequest<EmployeeResponse>;

public class CreateEmployeeCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordService passwordService,
    IGeocodingService geocodingService) : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employeeExist = await dbContext.Employees
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           (
                               x.Email == request.Request.Email
                               || x.PhoneNumber == request.Request.PhoneNumber
                           ),
                cancellationToken);

        if (employeeExist)
        {
            throw new ResourceIsAlreadyExistException(
                $"Email ({request.Request.Email}) or Number ({request.Request.PhoneNumber}) already exists");
        }

        await geocodingService.GetCoordinatesAsync(request.Request.Address);

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);

        var newEmployee = EmployeeMappers.ToEmployee(request.Request);
        newEmployee.PasswordHash = passwordHash;
        newEmployee.PharmacyId = request.PharmacyId;
        await dbContext.Employees.AddAsync(newEmployee, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return EmployeeMappers.ToEmployeeResponse(newEmployee);
    }
}