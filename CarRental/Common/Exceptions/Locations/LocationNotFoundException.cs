using System.Net;
using CarRental.Common.Exceptions.Cars;

namespace CarRental.Common.Exceptions.Locations;

public class LocationNotFoundException : CarRentalException
{
    public Guid Id { get; }
    public LocationNotFoundException(Guid id) : base($"Location id {id} not found")
    {
        Id = id;
    }

    public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}