using LegacyUserSystem.Entities;
using LegacyUserSystem.Exceptions;
using LegacyUserSystem.Services.Interfaces.User;
using Microsoft.AspNetCore.Identity;

namespace LegacyUserSystem.Services.User;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), username);
        }

        return user;
    }
}