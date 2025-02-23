namespace LegacyUserSystem.Models;

public class AuthenticatedResult
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}