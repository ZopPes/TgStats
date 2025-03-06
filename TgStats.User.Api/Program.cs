using TgStats.User.Api.Endpoint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitDefalt();
builder.Services.InitServices();
builder.Services.InitData(builder.Configuration);
builder.Services.InitAuth(builder.Configuration);

var app = builder.Build();

app.UserAuth();
app.UseDefalt();
app.AddUserEndpoint();

app.Run();