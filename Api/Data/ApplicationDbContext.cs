using ArknightsApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Operator> Operators { get; set; }
    
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subclass> Subclasses { get; set; }
}