using CarRental.Common;
using CarRental.Common.CQRS;
using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Features.Cars.GetAvailabilityCars;

public sealed record GetAvailabilityCarsQuery(
    DateTime StartDate,
    DateTime EndDate,
    Guid? LocationId = null,
    string? Model = null) : IQuery<List<AvailabilityCarResponse>>;

public sealed class GetAvailabilityCars : IQueryHandler<GetAvailabilityCarsQuery, List<AvailabilityCarResponse>>
{
    private readonly IAppDbContext _context;
    public GetAvailabilityCars(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<List<AvailabilityCarResponse>> Handle(GetAvailabilityCarsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cars
            .Include(c => c.CurrentLocation)
            .Where(c => !_context.Reservations.Any(r =>
                r.CarId == c.Id &&
                r.Status != ReservationStatus.Cancelled &&
                r.PickupDate <= request.EndDate &&
                r.ReturnDate >= request.StartDate
            ));
        
        if (request.LocationId.HasValue)
            query = query.Where(c => c.CurrentLocationId == request.LocationId);
       
        if (!string.IsNullOrWhiteSpace(request.Model))
            query = query.Where(c => EF.Functions.Like(c.Model.Name, request.Model));
        
        var availableCars = await query
            .Select(c => new AvailabilityCarResponse(
                c.Id, 
                c.Model.Name, 
                c.CurrentLocation.Name, 
                c.DailyRate))
            .ToListAsync(cancellationToken);
        
        return availableCars;
    }
}