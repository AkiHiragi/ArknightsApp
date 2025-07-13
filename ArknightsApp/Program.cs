using ArknightsApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.ConfigureApplication();
await app.SeedDatabaseAsync();

app.Run();