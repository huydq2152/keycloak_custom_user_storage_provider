using Microsoft.AspNetCore.Identity;

namespace LegacyUserSystem.Entities;

public class AppRole: IdentityRole<int>
{
    public string DisplayName { get; set; }
}