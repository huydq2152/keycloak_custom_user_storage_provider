using Microsoft.EntityFrameworkCore;

namespace LegacyUserSystem.Persistence.Seed;

public class ApplicationContextSeed(ILogger<ApplicationContextSeed> logger, ApplicationDbContext dbContext)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (dbContext.Database.IsSqlServer())
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError("An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await new RolesAndUsersCreator(dbContext).Create();
    }
}