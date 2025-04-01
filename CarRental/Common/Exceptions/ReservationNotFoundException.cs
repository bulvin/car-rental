using System.Net;
using CarRental.Common.Exceptions.Cars;

namespace CarRental.Common.Exceptions;

public class ReservationNotFoundException : CarRentalException
{
    public string Id { get; }
    public ReservationNotFoundException(string id) : base($"Reservation id {id} not found")
    {
        Id = id;
    }

    public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}