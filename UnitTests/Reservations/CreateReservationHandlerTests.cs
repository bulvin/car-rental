using CarRental.Common;
using CarRental.Common.Exceptions;
using CarRental.Data.Entities;
using CarRental.Features.Reservations;
using CarRental.Features.Reservations.CreateReservation;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;

namespace UnitTests.Reservations;

public class CreateReservationHandlerTests
{
    private readonly Mock<IAppDbContext> _contextMock;
    private readonly CreateReservation _handler;

    public CreateReservationHandlerTests()
    {
        _contextMock = new Mock<IAppDbContext>();
        _handler = new CreateReservation(_contextMock.Object);
    }

    [Fact]
    public async Task ShouldThrowCarReservationOverlapException_WhenRequestDatesOverlap()
    {
        var carId = Guid.NewGuid();
        var pickupDate = new DateTime(2025, 04, 10, 10, 0, 0);
        var returnDate = new DateTime(2025, 04, 12, 10, 0, 0);
        var pickupLocationId = Guid.NewGuid();
        
        var customer = new Customer() { Id = Guid.NewGuid(), Email = "abc@test.com" , FirstName = "abc", LastName = "test" };
        var command = new CreateReservationCommand(
            CarId: carId,
            PickupLocationId: pickupLocationId,
            ReturnLocationId: pickupLocationId,
            PickupDate: pickupDate,
            ReturnDate: returnDate,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email);

        List<Location> locations = [new() {Id = pickupLocationId, Name = "Palma Airport"}];

        List<Car> cars =
        [
            new()
            {
                Id = carId,
                DailyRate = 100.00m,
                CurrentLocationId = pickupLocationId,
                Model = CarModel.Model3,
                Color = "Red",
                Year = 2024,
            }
        ];
        var overlappingReservation = new Reservation
        {
            CarId = carId,
            PickupDate = new DateTime(2025, 04, 11, 10, 0, 0),
            ReturnDate = new DateTime(2025, 04, 13, 10, 0, 0),
            Status = ReservationStatus.Reserved
        };
        List<Reservation> reservations = [overlappingReservation];
        List<Customer> customers = [customer];
        
        _contextMock.Setup(x => x.Customers).ReturnsDbSet(customers);
        _contextMock.Setup(x => x.Cars).ReturnsDbSet(cars);
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(reservations);
        _contextMock.Setup(x => x.Locations).ReturnsDbSet(locations);
        
        await Should.ThrowAsync<CarReservationOverlapException>(async () =>
            await _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task ShouldCreateReservationSuccessfully_WhenDatesDoNotOverlap()
    {
        var carId = Guid.NewGuid();
        var locationId = Guid.NewGuid();
        var pickupDate = new DateTime(2025, 04, 10, 10, 0, 0);
        var returnDate = new DateTime(2025, 04, 12, 10, 0, 0);
        var customer = new Customer() { Id = Guid.NewGuid(), Email = "abc@test.com" , FirstName = "abc", LastName = "test" };
        var command = new CreateReservationCommand(
            CarId: carId,
            PickupLocationId: locationId,
            ReturnLocationId: locationId,
            PickupDate: pickupDate,
            ReturnDate: returnDate,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            Email: customer.Email);
        
        List<Car> cars = [new (){ Id = carId, DailyRate = 100, CurrentLocationId = locationId}];
        List<Location> locations = [new() {Id = locationId, Name = "Palma Airport"}];
        List<Reservation> reservations = [];
        List<Customer> customers = [customer];
        
        _contextMock.Setup(x => x.Customers).ReturnsDbSet(customers);
        _contextMock.Setup(x => x.Cars).ReturnsDbSet(cars);
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(reservations);
        _contextMock.Setup(x => x.Locations).ReturnsDbSet(locations);
        _contextMock.Setup(x => x.Reservations.Add(It.IsAny<Reservation>()))
            .Callback<Reservation>(r => r.Id = Guid.NewGuid());

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.ShouldNotBe(Guid.Empty);
    }
}