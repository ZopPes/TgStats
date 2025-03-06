using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgStats.user.Domain;

namespace TgStats.User.Infrastructure.Data.EF.Configurations;

internal class UserTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder) 
    {
        builder.Property(u => u.Password).HasConversion(u => u.Value, u => new(u));
        builder.Property(u => u.ID).HasConversion(u => u.Value, u => new(u));
    }
}