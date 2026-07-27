using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeesByNumberQuery(
    long PharmacyId,
    string Number
) : IRequest<EmployeeResponse>;

public class GetEmployeeByNumberQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<GetEmployeesByNumberQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeesByNumberQuery request, CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PhoneNumber == request.Number, cancellationToken);
        if (employees is null)
        {
            throw new RecourseNotFoundException($"Employee with this number {request.Number}  not found ");
        }

        return mapper.Map<EmployeeResponse>(employees);
    }
}