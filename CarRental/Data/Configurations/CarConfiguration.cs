using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Model)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.Color)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.Year)
            .IsRequired();
        
        builder.Property(c => c.DailyRate)
            .IsRequired();
        
        builder.HasOne(c => c.CurrentLocation)
            .WithMany()
            .HasForeignKey(c => c.CurrentLocationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}