using Tasks.Manager.Admin.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment.EnvironmentName);

var app = builder.Build();

app.ConfigureApp();

await app.RunAsync();
