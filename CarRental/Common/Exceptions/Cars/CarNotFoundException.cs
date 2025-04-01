using System.Net;
using CarRental.Common.Exceptions.Cars;

namespace CarRental.Common.Exceptions;

public class CarNotFoundException : CarRentalException
{
    public Guid Id { get; }
    public CarNotFoundException(Guid id) : base($"Car ID {id} not found")
    {
        Id = id;
    }

    public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}