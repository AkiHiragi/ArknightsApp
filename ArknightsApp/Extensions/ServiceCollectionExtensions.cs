using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Mapping;
using ArknightsApp.Services;
using ArknightsApp.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ArknightsApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                            IConfiguration          configuration)
    {
        // Controllers
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arknights API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name   = "Authorization",
                In     = ParameterLocation.Header,
                Type   = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        // Database
        services.AddDbContext<ArknightsDbContext>(options =>
        {
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"));
        });

        // Repositories
        services.AddScoped<IOperatorRepository, OperatorRepository>();

        // AutoMapper
        services.AddAutoMapper(typeof(OperatorProfile), typeof(UserProfile));

        // Validators
        services.AddScoped<IValidator<OperatorCreateDto>, OperatorCreateValidator>();
        services.AddScoped<IValidator<StatsRequestDto>, StatRequestValidator>();
        services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestValidator>();

        // Services
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ITalentService, TalentService>();
        services.AddScoped<IReferenceService, ReferenceService>();
        services.AddScoped<IAuthService, AuthService>();

        // JWT Authentication
        services.AddJwtAuthentication(configuration);

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
                                                           IConfiguration          configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
                         ValidateIssuer = true,
                         ValidIssuer = jwtSettings["Issuer"],
                         ValidateAudience = true,
                         ValidAudience = jwtSettings["Audience"],
                         ValidateLifetime = true,
                         ClockSkew = TimeSpan.Zero
                     };
                 });

        services.AddAuthorization();
        return services;
    }
}