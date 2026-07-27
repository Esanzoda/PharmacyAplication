using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Commands;

public record UpdateEmployeeCommand(
    long PharmacyId,
    long EmployeeId,
    EmployeeRequest Request) : IRequest<EmployeeResponse>;

public class UpdateEmployeeHandler(
    IApplicationDbContext dbContext,
    IMapper mapper
) : IRequestHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.EmployeeId,
                cancellationToken);
        if (employee is null)
        {
            throw new RecourseNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        var employeeExist = await dbContext.Employees
            .AnyAsync(x => x.PharmacyId == request.PharmacyId &&
                           x.Id != request.EmployeeId &&
                           (
                               x.Email == request.Request.Email ||
                               x.PhoneNumber == request.Request.PhoneNumber
                           ), cancellationToken);
        if (employeeExist)
        {
            throw new RecourseIsAlreadyExistException(
                $"Email: {request.Request.Email} or Number{request.Request.PhoneNumber}already exists");
        }

        if (request.Request.Role == Role.Customer)
        {
            throw new BusinessException("Cant update employee status to  customer status");
        }

        mapper.Map(request.Request, employee);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<EmployeeResponse>(employee);
    }
}