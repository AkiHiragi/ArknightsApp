using ArknightsApp.Data;
using ArknightsApp.Mappings;
using ArknightsApp.Services;
using ArknightsApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArknightsApp.Extensions;

public static class AddServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<ISubclassService, SubclassService>();
        services.AddScoped<IImageProcessingService, ImageProcessingService>();
    }

    public static void AddMappingsAndValidation(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<OperatorDtoValidator>();
    }
    
    public static void AddPostgreSqlConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
    
    public static void AddIdentityUsersAndRoles(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
    }
}