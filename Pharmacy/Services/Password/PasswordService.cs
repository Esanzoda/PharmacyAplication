namespace Pharmacy.Services.Password;

public interface IPasswordService
{
    Task<string> PasswordHash(string password);
    Task<bool> PasswordVerify(string password, string passwordHash);
}

public class PasswordService : IPasswordService
{
    public Task<string> PasswordHash(string password)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public Task<bool> PasswordVerify(string password, string passwordHash)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordHash));
    }
}