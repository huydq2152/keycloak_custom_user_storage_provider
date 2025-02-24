using System.Security.Claims;
using LegacyUserSystem.Constances.Auth;
using LegacyUserSystem.Services.Interfaces.User;

namespace LegacyUserSystem.Services.User;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(UserClaims.Id);
            return userId != null ? Convert.ToInt32(userId) : null;
        }
    }
}