using LegacyUserSystem.Extensions;
using LegacyUserSystem.Persistence.Seed;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "CorsPolicy";
builder.AddAppConfigurations();

builder.Services.AddServices();
builder.Services.AddLogging();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddIdentity();

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.IgnoreObsoleteActions();
    options.IgnoreObsoleteProperties();
    options.CustomSchemaIds(type => type.FullName);
            
    options.CustomOperationIds(description =>
        description.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
    options.SwaggerDoc("LegacyUserSystemApi", new OpenApiInfo
    {
        Title = "LegacyUserSystem Api",
        Version = "v1",
        Description = "API for LegacyUserSystem project",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddCorsPolicy(builder.Configuration, corsPolicy);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("LegacyUserSystemApi/swagger.json", "LegacyUserSystem Api");
        options.DisplayOperationId();
        options.DisplayRequestDuration();
    });
    app.UseDeveloperExceptionPage();
}

using var scope = app.Services.CreateScope();
var applicationContextSeed = scope.ServiceProvider.GetRequiredService<ApplicationContextSeed>();
await applicationContextSeed.InitialiseAsync();
await applicationContextSeed.SeedAsync();

app.UseCors(corsPolicy);
app.UseRouting();
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();