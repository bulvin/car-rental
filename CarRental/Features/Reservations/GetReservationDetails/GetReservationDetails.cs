using CarRental.Common;
using CarRental.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using ReservationNotFoundException = CarRental.Common.Exceptions.Reservations.ReservationNotFoundException;

namespace CarRental.Features.Reservations.GetReservationDetails;

public record GetReservationDetailsQuery(string Id) : IQuery<ReservationDetailsResponse>;

public class GetReservationDetails : IQueryHandler<GetReservationDetailsQuery, ReservationDetailsResponse>
{
    private readonly IAppDbContext _context;

    public GetReservationDetails(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<ReservationDetailsResponse> Handle(GetReservationDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reservations
            .AsNoTracking()
            .Include(r => r.Car)
            .Include(r => r.PickupLocation)
            .Include(r => r.ReturnLocation)
            .AsQueryable();

        var reservation = Guid.TryParse(request.Id, out var reservationId)
            ? await query.FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken)
            : await query.FirstOrDefaultAsync(r => r.ReservationNumber == request.Id, cancellationToken);

        if (reservation is null)
            throw new ReservationNotFoundException(request.Id);

        return new ReservationDetailsResponse
        {
            Id = reservation.Id,
            ReservationNumber = reservation.ReservationNumber,
            PickupDate = reservation.PickupDate,
            ReturnDate = reservation.ReturnDate,
            TotalCost = reservation.TotalCost,
            Car = new CarReservationDetailsResponse(
                reservation.Car.Id, 
                reservation.Car.Model.Name,
                reservation.Car.Color),
            PickupLocationName = reservation.PickupLocation.Name,
            ReturnLocationName = reservation.ReturnLocation.Name,
            Status = reservation.Status
        };
    }
}