using NewsWebsite.API.Dtos;

namespace NewsWebsite.API.Services
{
    public interface IAuthService
    {
        public Task<AuthDto> LoginAsync(string username, string password);
    }
}
