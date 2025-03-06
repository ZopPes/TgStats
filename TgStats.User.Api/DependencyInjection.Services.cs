using Microsoft.AspNetCore.Identity;
using TgStats.user.Application.Implementation;
using TgStats.user.Application;

internal static partial class DependencyInjection
{
    internal static IServiceCollection InitServices(this IServiceCollection services)
         => services.AddScoped<IUserService, UserService>()
         .AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

}
