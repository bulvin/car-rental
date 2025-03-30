using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Common;

public interface IAppDbContext
{
    DbSet<Car> Cars { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<Location> Locations { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}