using System.Text;
using LegacyUserSystem.Configurations;
using LegacyUserSystem.Entities;
using LegacyUserSystem.Persistence;
using LegacyUserSystem.Persistence.Seed;
using LegacyUserSystem.Services;
using LegacyUserSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LegacyUserSystem.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ApplicationContextSeed>()
            .AddScoped<ITokenService, TokenService>();
    }
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });
    }
    
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });
    }
    
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtTokenSettingsSection = configuration.GetSection("JwtTokenSettings");

        services.Configure<JwtTokenSettings>(jwtTokenSettingsSection);

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;

            var jwtTokenSettings = jwtTokenSettingsSection.Get<JwtTokenSettings>();

            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0),
                ValidIssuer = jwtTokenSettings.Issuer,
                ValidAudience = jwtTokenSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Key))
            };
        });
    }
    
    public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration,
        string corsPolicy)
    {
        var allowedOrigins = configuration["AllowedOrigins"];
        if (string.IsNullOrEmpty(allowedOrigins))
            throw new ArgumentNullException("AllowedOrigins is not configured");
        services.AddCors(o => o.AddPolicy(corsPolicy, builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(allowedOrigins)
                .AllowCredentials();
        }));
    }
}