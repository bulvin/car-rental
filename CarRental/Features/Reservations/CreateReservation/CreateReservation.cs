using CarRental.Common;
using CarRental.Common.CQRS;
using CarRental.Common.Exceptions;
using CarRental.Common.Exceptions.Locations;
using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Features.Reservations.CreateReservation;

public record CreateReservationCommand(
    Guid CarId, 
    Guid PickupLocationId,
    Guid ReturnLocationId,
    DateTime PickupDate,
    DateTime ReturnDate,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber = null
    ) : ICommand<Guid>;

public class CreateReservation : ICommandHandler<CreateReservationCommand, Guid>
{ 
    private readonly IAppDbContext _context;

    public CreateReservation(IAppDbContext context)
    {
          _context = context;
    } 
    
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var (carId, pickupLocationId, returnLocationId, pickupDate, returnDate, firstName, lastName, email, phoneNumber) = request;
        var pickupDateUtc = pickupDate.ToUniversalTime().RoundToFullHour();
        var returnDateUtc = returnDate.ToUniversalTime().RoundToFullHour();
        
        var returnLocationExists = await _context.Locations
            .AnyAsync(l => l.Id == returnLocationId, cancellationToken);

        if (!returnLocationExists)
            throw new LocationNotFoundException(returnLocationId);
        
        var isCarAvailable = !await _context.Reservations.AnyAsync(r => 
                r.CarId == carId && 
                r.Status == ReservationStatus.Reserved &&
                (r.PickupDate <= returnDateUtc && r.ReturnDate >= pickupDateUtc),
            cancellationToken);

        if (!isCarAvailable)
            throw new CarReservationOverlapException(carId, pickupDateUtc, returnDateUtc);
        
        var car = await _context.Cars
            .Where(c => c.Id == carId)
            .Select(c => new 
            { 
                c.Id, 
                c.DailyRate, 
                c.CurrentLocationId
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CarNotFoundException(carId);

        if (car.CurrentLocationId != pickupLocationId)
            throw new LocationNotFoundException(pickupLocationId);
        
        var days = Math.Max(1, (int)Math.Ceiling((returnDateUtc - pickupDateUtc).TotalDays));
        var totalCost = car.DailyRate * days;
        
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);

        if (customer is null)
        {
            customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber ?? string.Empty,
            };
            _context.Customers.Add(customer);
        }
        
        var reservation = new Reservation
        {
            CustomerId = customer.Id,
            ReservationNumber = Reservation.GenerateReservationNumber(),
            CarId = carId,
            PickupLocationId = pickupLocationId,
            ReturnLocationId = returnLocationId,
            PickupDate = pickupDateUtc,
            ReturnDate = returnDateUtc,
            TotalCost = totalCost,
            Status = ReservationStatus.Reserved
        };
        
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return reservation.Id;
    }
}