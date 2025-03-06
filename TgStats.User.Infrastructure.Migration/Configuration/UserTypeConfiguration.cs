using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgStats.User.Infrastructure.Migrations.Entities;

namespace TgStats.User.Infrastructure.Migrations.Configuration;

internal class UserTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
    }
}