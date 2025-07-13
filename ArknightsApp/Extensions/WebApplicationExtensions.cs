using ArknightsApp.Data;

namespace ArknightsApp.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Static files
        app.UseStaticFiles();

        // Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arknights API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        // Middleware pipeline
        app.UseCors("AllowReactApp");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        using var scope   = app.Services.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<ArknightsDbContext>();
        await DbSeeder.SeedAsync(context);
        return app;
    }
}