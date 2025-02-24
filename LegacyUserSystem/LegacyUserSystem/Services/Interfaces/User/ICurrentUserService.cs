namespace LegacyUserSystem.Services.Interfaces.User;

public interface ICurrentUserService
{
    int? UserId { get; }
}