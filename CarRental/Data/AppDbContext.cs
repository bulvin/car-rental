using Microsoft.EntityFrameworkCore;

namespace CarRental.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("CarRental");
        base.OnModelCreating(modelBuilder);
    }
}