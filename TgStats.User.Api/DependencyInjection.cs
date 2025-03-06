using Scalar.AspNetCore;

internal static partial class DependencyInjection
{
    internal static IServiceCollection InitDefalt(this IServiceCollection services)
        => services
        .AddOptions()
        .AddLogging()
        .AddOpenApi()
        ;

    internal static IApplicationBuilder UseDefalt<Tapp>(this Tapp app)
        where Tapp : IHost, IEndpointRouteBuilder, IApplicationBuilder
    {
        // Configure the HTTP request pipeline.
        if(app.Services.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapGet("/ping", () => Results.Ok())
            .AllowAnonymous()
            .WithDescription("Для проверки соединения.")
            .WithSummary("ping 200");

        return app.UseHttpsRedirection();
    }
}
