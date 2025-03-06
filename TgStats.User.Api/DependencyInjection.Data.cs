using Microsoft.EntityFrameworkCore;
using TgStats.user.Domain.Repositories;
using TgStats.User.Infrastructure.Data.EF;
using TgStats.User.Infrastructure.Repositories;

internal static partial class DependencyInjection
{
    internal static IServiceCollection InitData(this IServiceCollection services, IConfiguration configuration)
         => services.AddScoped<IUserRepository, UserRepository>()
             .AddDbContext<UserDbContext>(options => options
             .UseNpgsql(configuration["ConnectionStrings:UserDbContext"]));

}
