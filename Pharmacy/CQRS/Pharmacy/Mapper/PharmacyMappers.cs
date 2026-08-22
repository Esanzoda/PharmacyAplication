using Pharmacy.CQRS.Pharmacy.Models;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;

namespace Pharmacy.CQRS.Pharmacy.Mapper;

public static class PharmacyMappers
{
    public static PharmacyEntity ToPharmacy(PharmacyRequest request)
    {
        return new PharmacyEntity
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            OpeningTime = request.OpeningTime,
            ClosingTime = request.ClosingTime
        };
    }

    public static PharmacyResponse ToPharmacyResponse(PharmacyEntity pharmacy)
    {
        return new PharmacyResponse
        {
            Id = pharmacy.Id,
            Name = pharmacy.Name,
            Address = pharmacy.Address,
            PhoneNumber = pharmacy.PhoneNumber,
            Email = pharmacy.Email,
            OpeningTime = pharmacy.OpeningTime,
            ClosingTime = pharmacy.ClosingTime
        };
    }

    public static List<PharmacyResponse> ToListPharmacyResponse(List<PharmacyEntity> pharmacies)
    {
        return pharmacies
            .Select(ToPharmacyResponse)
            .ToList();
    }
}