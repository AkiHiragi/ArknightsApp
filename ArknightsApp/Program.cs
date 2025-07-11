using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Mapping;
using ArknightsApp.Services;
using ArknightsApp.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arknights API", Version = "v1" });
});

builder.Services.AddDbContext<ArknightsDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration
               .GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();

builder.Services.AddAutoMapper(typeof(OperatorProfile));

builder.Services.AddScoped<IValidator<OperatorCreateDto>, OperatorCreateValidator>();
builder.Services.AddScoped<IValidator<StatsRequestDto>, StatRequestValidator>();

builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IReferenceService, ReferenceService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arknights API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ArknightsDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.Run();