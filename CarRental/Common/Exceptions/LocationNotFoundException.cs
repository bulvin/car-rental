using System.Net;

namespace CarRental.Common.Exceptions;

public class LocationNotFoundException : CarRentalException
{
    public Guid Id { get; }
    public LocationNotFoundException(Guid id) : base($"Location id {id} not found")
    {
        Id = id;
    }

    public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}