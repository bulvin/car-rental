using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data;

public static class DbSeeder
{
    public static async Task Seed(AppDbContext context)
    {
        if (await context.Cars.AnyAsync())
        {
            return;
        }
        
        var locations = new List<Location>
        {
            new Location { Id = Guid.NewGuid(), Name = "Palma Airport" },
            new Location { Id = Guid.NewGuid(), Name = "Palma City Center" },
            new Location { Id = Guid.NewGuid(), Name = "Alcudia" },
            new Location { Id = Guid.NewGuid(), Name = "Manacor" }
        };
        context.Locations.AddRange(locations);
        
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "abc",
                LastName = "abc",
                Email = "a@b.pl",
                PhoneNumber = "+123456789"
            },
            new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "a@c.pl",
                PhoneNumber = "+987654321"
            },
            new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Carlos",
                LastName = "García",
                Email = "a@f.pl",
                PhoneNumber = "+34678901234"
            }
        };
        context.Customers.AddRange(customers);

        var cars = new List<Car>
        {
            new Car
            {
                Id = Guid.NewGuid(),
                Model = new CarModel("Model S", 5),
                Color = "Red",
                Year = 2022,
                DailyRate = 99.99m,
                CurrentLocationId = locations[0].Id,
                IsAvailable = true
            },
            new Car
            {
                Id = Guid.NewGuid(),
                Model = new CarModel("Model 3", 5),
                Color = "Blue",
                Year = 2023,
                DailyRate = 79.99m,
                CurrentLocationId = locations[1].Id,
                IsAvailable = true
            },
            new Car
            {
                Id = Guid.NewGuid(),
                Model = new CarModel("Model X", 7),
                Color = "Black",
                Year = 2021,
                DailyRate = 129.99m,
                CurrentLocationId = locations[2].Id,
                IsAvailable = false
            }
        };
        context.Cars.AddRange(cars);
    
        var reservations = new List<Reservation>
        {
            new Reservation
            {
                Id = Guid.NewGuid(),
                ReservationNumber = Reservation.GenerateReservationNumber(),
                CustomerId = customers[0].Id,
                CarId = cars[2].Id,
                PickupLocationId = locations[2].Id,
                ReturnLocationId = locations[0].Id,
                PickupDate = DateTime.UtcNow.AddDays(1),
                ReturnDate = DateTime.UtcNow.AddDays(5),
                TotalCost = cars[2].DailyRate * 5,
                Status = ReservationStatus.Reserved
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                ReservationNumber = Reservation.GenerateReservationNumber(),
                CustomerId = customers[1].Id,
                CarId = cars[1].Id,
                PickupLocationId = locations[3].Id,
                ReturnLocationId = locations[1].Id,
                PickupDate = DateTime.UtcNow.AddDays(3),
                ReturnDate = DateTime.UtcNow.AddDays(7),
                TotalCost = cars[1].DailyRate * 4,
                Status = ReservationStatus.Reserved
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                ReservationNumber = Reservation.GenerateReservationNumber(),
                CustomerId = customers[2].Id,
                CarId = cars[0].Id,
                PickupLocationId = locations[2].Id,
                ReturnLocationId = locations[3].Id,
                PickupDate = DateTime.UtcNow.AddDays(2),
                ReturnDate = DateTime.UtcNow.AddDays(10),
                TotalCost = cars[0].DailyRate * 8,
                Status = ReservationStatus.Reserved
            }
        };
        context.Reservations.AddRange(reservations);
        
        await context.SaveChangesAsync();
    }
}