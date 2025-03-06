using Microsoft.EntityFrameworkCore;
using TgStats.User.Infrastructure.Migrations;
//Проект только для миграций. Функционала нет.
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<UserDbContextMigration>(options => options
        .UseNpgsql(configuration["ConnectionStrings:UserDbContext"]));
var app = builder.Build();
await using var serviceScope = app.Services.CreateAsyncScope();
var db = serviceScope
    .ServiceProvider
    .GetRequiredService<UserDbContextMigration>()
    .Database;
var pendingMigrations = await db.GetPendingMigrationsAsync();
if(!pendingMigrations.Any())
{
    return;
}

await db
    .MigrateAsync();
