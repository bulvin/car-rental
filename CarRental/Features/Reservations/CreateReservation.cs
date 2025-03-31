using CarRental.Common;
using CarRental.Common.CQRS;
using CarRental.Common.Exceptions;
using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Features.Reservations;

public record CreateReservationCommand(
    Guid CarId, 
    Guid PickupLocationId,
    Guid ReturnLocationId,
    DateTime PickupDate,
    DateTime ReturnDate
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
        var (carId, pickupLocationId, returnLocationId, pickupDate, returnDate) = request;
        var pickupDateUtc = pickupDate.ToUniversalTime().RoundToFullHour();
        var returnDateUtc = returnDate.ToUniversalTime().RoundToFullHour();
        
        var returnLocationExists = await _context.Locations
            .AnyAsync(l => l.Id == returnLocationId, cancellationToken);

        if (!returnLocationExists)
            throw new LocationNotFoundException(returnLocationId);
        
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
       
        var isCarAvailable = !await _context.Reservations.AnyAsync(r => 
                r.CarId == carId && 
                r.Status == ReservationStatus .Reserved &&
                (r.PickupDate <= returnDateUtc && r.ReturnDate >= pickupDateUtc),
            cancellationToken);

        if (!isCarAvailable)
            throw new CarReservationOverlapException(carId, pickupDateUtc, returnDateUtc);
        
        var days = Math.Max(1, (int)Math.Ceiling((returnDateUtc - pickupDateUtc).TotalDays));
        var totalCost = car.DailyRate * days;
        
        var reservation = new Reservation
        {
            CustomerId = new Guid("43962baa-64b3-4997-af15-f92e760c285a"),
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