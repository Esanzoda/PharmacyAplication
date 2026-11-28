namespace PharmacyService.Contract.Response;

public record LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}