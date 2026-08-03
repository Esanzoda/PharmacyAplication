using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Services.Password;

namespace Pharmacy.CQRS.Employee.Commands;

public record CreateEmployeeCommand(
    long PharmacyId,
    EmployeeRequest Request) : IRequest<EmployeeResponse>;

public class CreateEmployeeCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper,
    IPasswordService passwordService) : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
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
            throw new RecourseIsAlreadyExistException(
                $"Email ({request.Request.Email}) or Number ({request.Request.PhoneNumber}) already exists");
        }

        if (request.Request.Role is Role.Customer or Role.Deliver)
        {
            throw new BusinessException("Cant create employee with status customer");
        }

        var passwordHash = await passwordService.PasswordHash(request.Request.Password);
        var newEmployee = mapper.Map<Models.Employee>(request.Request);
        newEmployee.PasswordHash = passwordHash;
        newEmployee.PharmacyId = request.PharmacyId;
        await dbContext.Employees.AddAsync(newEmployee, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<EmployeeResponse>(newEmployee);
    }
}