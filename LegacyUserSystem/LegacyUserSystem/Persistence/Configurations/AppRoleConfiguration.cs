using LegacyUserSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LegacyUserSystem.Persistence.Configurations;

public class AppRoleConfiguration: IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("Identity_Roles");
        
        builder.Property(role => role.DisplayName)
            .HasMaxLength(200)
            .IsRequired();
    }
}