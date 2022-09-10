using RestApiBlog.Domain;

namespace RestApiBlog.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}
