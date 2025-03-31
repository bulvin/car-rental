using CarRental.Common;
using CarRental.Common.CQRS;
using CarRental.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Features.Cars.GetCarDetails;

public record GetCarDetailsQuery(Guid Id) : IQuery<CarDetailsResponse>;

public class GetCarDetails : IQueryHandler<GetCarDetailsQuery, CarDetailsResponse>
{
    private readonly IAppDbContext _context;

    public GetCarDetails(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<CarDetailsResponse> Handle(GetCarDetailsQuery request, CancellationToken cancellationToken)
    {
        var car = await _context.Cars
            .Include(c => c.CurrentLocation)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
                  throw new CarNotFoundException(request.Id);
        
        var response = new CarDetailsResponse(
            car.Id, 
            car.Model.Name, 
            car.Model.PassengerCapacity, 
            car.Color, 
            car.Year, 
            car.DailyRate, 
            car.CurrentLocation.Name);
        
        return response;
    }
}