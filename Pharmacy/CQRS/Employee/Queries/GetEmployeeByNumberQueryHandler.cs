using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByNumberQuery(
    long PharmacyId,
    string Number
) : IRequest<EmployeeResponse>;

public class GetEmployeeByNumberQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetEmployeeByNumberQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeByNumberQuery request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PhoneNumber == request.Number, cancellationToken);
        if (employee is null)
        {
            throw new RecourseNotFoundException($"Employee with this number {request.Number}  not found ");
        }

        return mapper.Map<EmployeeResponse>(employee);
    }
}