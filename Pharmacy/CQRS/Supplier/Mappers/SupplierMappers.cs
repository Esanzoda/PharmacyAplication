using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Deliver;

namespace Pharmacy.CQRS.Supplier.Mappers;

public static class SupplierMappers
{
    public static Domain.Models.Deliver.Supplier ToSupplier(SupplierRequest request)
    {
        return new Domain.Models.Deliver.Supplier()
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

    public static SupplierResponse ToSupplierResponse(Domain.Models.Deliver.Supplier supplier)
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

    public static List<SupplierResponse> ToSuppliersResponse(List<Domain.Models.Deliver.Supplier> suppliers)
    {
        return suppliers
            .Select(ToSupplierResponse)
            .ToList();
    }

    public static void ToUpdateSupplier(UpdateSupplierRequest request,
        Domain.Models.Deliver.Supplier supplier)
    {
        supplier.Name = request.Name;
        supplier.VehicleNumber = request.VehicleNumber.ToUpper();
        supplier.Email = request.Email.ToLower();
        supplier.PhoneNumber = request.PhoneNumber;
        supplier.VehicleType = request.VehicleType;
    }
}