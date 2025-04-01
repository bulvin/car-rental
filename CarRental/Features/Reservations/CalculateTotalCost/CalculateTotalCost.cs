using CarRental.Common;
using CarRental.Common.CQRS;
using CarRental.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Features.Reservations.CalculateTotalCost;

public record CalculateTotalCostQuery(Guid CarId, DateTime PickupDate, DateTime ReturnDate) : IQuery<ReservationCostResponse>;
    
public class CalculateTotalCost : IQueryHandler<CalculateTotalCostQuery, ReservationCostResponse>
{
    private readonly IAppDbContext _context;

    public CalculateTotalCost(IAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ReservationCostResponse> Handle(CalculateTotalCostQuery request, CancellationToken cancellationToken)
    {
        var pickupDateUtc = request.PickupDate.ToUniversalTime().RoundToFullHour();
        var returnDateUtc = request.ReturnDate.ToUniversalTime().RoundToFullHour();
        
        var car =  await _context.Cars
            .Where(c => c.Id == request.CarId)
            .Select(c => new { dailyRate = c.DailyRate })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CarNotFoundException(request.CarId);
        
        var days = Math.Max(1, (int)Math.Ceiling((returnDateUtc - pickupDateUtc).TotalDays));
        var totalCost =  car.dailyRate * days;
        
        return new ReservationCostResponse(days, totalCost);
    }
}