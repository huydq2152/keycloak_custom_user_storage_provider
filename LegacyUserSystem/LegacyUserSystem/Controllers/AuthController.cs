using System.Security.Claims;
using LegacyUserSystem.Constances.Auth;
using LegacyUserSystem.Entities;
using LegacyUserSystem.Models;
using LegacyUserSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace LegacyUserSystem.Controllers;

public class AuthController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService)
    : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
    {
        //Authentication
        if (request == null)
        {
            return BadRequest("Request is invalid");
        }

        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null || user.IsActive == false || user.LockoutEnabled)
        {
            return BadRequest("Username is not correct");
        }

        var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);
        if (!result.Succeeded)
        {
            return BadRequest("Username or password is not correct");
        }

        //Authorization
        var roles = await userManager.GetRolesAsync(user);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(UserClaims.Id, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(UserClaims.FirstName, user.FirstName),
            new Claim(UserClaims.LastName, user.LastName),
            new Claim(UserClaims.Roles, string.Join(";", roles)),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
        await userManager.UpdateAsync(user);

        return Ok(new AuthenticatedResult()
        {
            Token = accessToken,
            RefreshToken = refreshToken
        });
    }
}