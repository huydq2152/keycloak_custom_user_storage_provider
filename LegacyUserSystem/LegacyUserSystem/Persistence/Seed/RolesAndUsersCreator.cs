using LegacyUserSystem.Constances.Auth;
using LegacyUserSystem.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LegacyUserSystem.Persistence.Seed;

public class RolesAndUsersCreator(ApplicationDbContext dbContext)
{
    public async Task Create()
    {
        await CreateRolesAndUsers();
    }

    private async Task CreateRolesAndUsers()
    {
        // Admin role 
        var adminRole = await dbContext.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.Name == Roles.AdminRoleName);
        if (adminRole == null)
        {
            var role = new AppRole()
            {
                Name = Roles.AdminRoleName,
                NormalizedName = Roles.AdminNormalizedName,
                DisplayName = Roles.AdminDisplayName
            };
            adminRole = (await dbContext.Roles.AddAsync(role)).Entity;
            await dbContext.SaveChangesAsync();
        }

        // Default users
        var adminUser = await dbContext.Users.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.UserName == Users.AdminUserName);
        if (adminUser == null)
        {
            var user = new AppUser()
            {
                FirstName = "Dang",
                LastName = "Huy",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                UserName = Users.AdminUserName,
                NormalizedUserName = Users.NormalizedAdminUserName,
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false
            };
            
            var passwordHasher = new PasswordHasher<AppUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "123qwe");

            adminUser = (await dbContext.Users.AddAsync(user)).Entity;
            await dbContext.SaveChangesAsync();
            
            //Assign admin role to admin user
            dbContext.UserRoles.Add(new IdentityUserRole<int>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
            await dbContext.SaveChangesAsync();
        }
    }
}