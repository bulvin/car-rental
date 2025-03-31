using CarRental.Common;
using CarRental.Data.Entities;
using CarRental.Features.Cars.GetAvailabilityCars;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;

namespace UnitTests.Cars;

public class GetAvailabilityCarsHandlerTests
{
    private readonly Mock<IAppDbContext> _contextMock;
    private readonly GetAvailabilityCars _handler;

    public GetAvailabilityCarsHandlerTests()
    {
        _contextMock = new Mock<IAppDbContext>();
        _handler = new GetAvailabilityCars(_contextMock.Object);
    }

    [Fact]
    public async Task ShouldReturnOnlyAvailableCars_WhenCarsAreReservedInGivenPeriod()
    {
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(1);

        var locationId = Guid.NewGuid();
        var carId1 = Guid.NewGuid();
        var carId2 = Guid.NewGuid();

        var testLocation = new Location { Id = locationId, Name = "Test Location" };

        var cars = new List<Car>
        {
            new() { 
                Id = carId1, 
                Model = CarModel.Model3, 
                CurrentLocationId = locationId, 
                CurrentLocation = testLocation, 
                Year = 2020, 
                DailyRate = 90.00m, 
                Color = "red" 
            },
            new() { 
                Id = carId2, 
                Model = CarModel.ModelS, 
                CurrentLocationId = locationId, 
                CurrentLocation = testLocation, 
                Year = 2020, 
                DailyRate = 90.00m, 
                Color = "red" 
            }
        }.AsQueryable();

        var reservations = new List<Reservation>
        {
            new() { 
                CarId = carId1, 
                PickupDate = startDate.AddHours(-1), 
                ReturnDate = endDate.AddHours(1), 
                Status = ReservationStatus.Reserved
            }
        }.AsQueryable();

        _contextMock.Setup(x => x.Cars).ReturnsDbSet(cars);
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(reservations);

        var query = new GetAvailabilityCarsQuery(startDate, endDate);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Model.ShouldBe(CarModel.ModelS.Name);
    }

    [Fact]
    public async Task ShouldReturnAllCarsFromPalma_WhenLocationIsSpecified()
    {
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(1);

        var palmaLocation = new Location { Id = Guid.NewGuid(), Name = "Palma" };
        var otherLocation = new Location { Id = Guid.NewGuid(), Name = "Alcudia" };

        var cars = new List<Car>
        {
            new() { Id = Guid.NewGuid(), Model = CarModel.Model3, CurrentLocationId = palmaLocation.Id, CurrentLocation = palmaLocation, Year = 2020, DailyRate = 90.00m, Color = "red" },
            new() { Id = Guid.NewGuid(), Model = CarModel.ModelS, CurrentLocationId = palmaLocation.Id, CurrentLocation = palmaLocation, Year = 2020, DailyRate = 90.00m, Color = "red" },
            new() { Id = Guid.NewGuid(), Model = CarModel.ModelX, CurrentLocationId = otherLocation.Id, CurrentLocation = otherLocation, Year = 2022, DailyRate = 150m, Color = "black" }
        }.AsQueryable();

        var reservations = new List<Reservation>().AsQueryable();

        _contextMock.Setup(x => x.Cars).ReturnsDbSet(cars);
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(reservations);

        var query = new GetAvailabilityCarsQuery(startDate, endDate, palmaLocation.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(c => c.LocationName == palmaLocation.Name).ShouldBeTrue();
    }
}