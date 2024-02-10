using DatingApp.Domain.Models;


namespace DatingApp.Application.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
