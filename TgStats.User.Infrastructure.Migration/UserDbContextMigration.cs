using Microsoft.EntityFrameworkCore;

namespace TgStats.User.Infrastructure.Migrations;

public class UserDbContextMigration(DbContextOptions<UserDbContextMigration> options) 
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContextMigration).Assembly);
    }
}