using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Domain.Entities;
using PharmacyService.Contract.Grpc;


namespace PharmacyService.Api.Application.CQRS.Pharmacy.Mapper;

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

    public static PharmacyGrpcResponse ToPharmacyGrpcResponse(PharmacyEntity pharmacy)
    {
        return new PharmacyGrpcResponse
        {
            Id = pharmacy.Id,
            Email = pharmacy.Email,
        };
    }

    public static PharmaciesGrpcResponse ToListPharmacyGrpcResponse(List<PharmacyEntity> pharmacies)
    {
        var response = new PharmaciesGrpcResponse();
        response.Pharmacies.AddRange(
            pharmacies.Select(ToPharmacyGrpcResponse));
        return response;
    }
}