using PharmacyService.Contract.Response;
using Refit;

namespace PharmacyService.ApiClient.Clients.IPharmacyApi;

public interface IPharmacyEndpoint
{
    [Patch("/api/Pharmacy/UpdateName")]
    Task<PharmacyResponse> UpdateName(
        [Body] string newName,
        long pharmacyId);

    [Patch("/api/Pharmacy/UpdateAddress")]
    Task<PharmacyResponse> UpdateAddress(
        [Body] string nawAddress, long pharmacyId);

    [Patch("/api/Pharmacy/UpdateEmail")]
    Task<PharmacyResponse> UpdateEmail(
        [Body] string newEmail, long pharmacyId);

    [Patch("/api/Pharmacy/UpdatePhoneNumber")]
    Task<PharmacyResponse> UpdatePhoneNumber(
        [Body] string nawNumber,
        long pharmacyId);
}