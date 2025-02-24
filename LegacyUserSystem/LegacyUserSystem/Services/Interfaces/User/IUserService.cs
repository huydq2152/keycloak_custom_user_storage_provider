using LegacyUserSystem.Entities;

namespace LegacyUserSystem.Services.Interfaces.User;

public interface IUserService
{
    Task<AppUser> GetUserByUsernameAsync(string username);
}