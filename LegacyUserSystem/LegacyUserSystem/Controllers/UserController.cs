using LegacyUserSystem.Entities;
using LegacyUserSystem.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace LegacyUserSystem.Controllers;

public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<AppUser>> GetUserByUsernameAsync(string username)
    {
        var result = await _userService.GetUserByUsernameAsync(username);
        return Ok(result);
    }
}