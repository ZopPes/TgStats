using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();

app.MapGet("/ping", () => Results.Ok())
    .WithDescription("Для проверки соединения.")
    .WithSummary("пустой ответ 200");

app.Run();