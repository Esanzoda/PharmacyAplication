using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using Role = PharmacyService.Api.Domain.Enums.Role;

namespace PharmacyService.Api.Application.CQRS.Supplier.Mappers;

public static class SupplierMappers
{
    public static Domain.Entities.Supplier ToSupplier(SupplierRequest request)
    {
        return new Domain.Entities.Supplier
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email.ToLower(),
            Role = Role.Supplier,
            VehicleType = request.VehicleType,
            VehicleNumber = request.VehicleNumber.ToUpper()
        };
    }

    public static SupplierResponse ToSupplierResponse(Domain.Entities.Supplier supplier)
    {
        return new SupplierResponse()
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Address = supplier.Address,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
            VehicleType = supplier.VehicleType,
            VehicleNumber = supplier.VehicleNumber,
            CreatedAt = supplier.CreatedAt,
            UpdateAt = supplier.UpdateAt,
            IsDeleted = supplier.IsDeleted
        };
    }

    public static List<SupplierResponse> ToSuppliersResponse(List<Domain.Entities.Supplier> suppliers)
    {
        return suppliers
            .Select(ToSupplierResponse)
            .ToList();
    }

    public static void ToUpdateSupplier(UpdateSupplierRequest request,
        Domain.Entities.Supplier supplier)
    {
        supplier.Name = request.Name;
        supplier.VehicleNumber = request.VehicleNumber.ToUpper();
        supplier.Email = request.Email.ToLower();
        supplier.PhoneNumber = request.PhoneNumber;
        supplier.VehicleType = request.VehicleType;
    }
}