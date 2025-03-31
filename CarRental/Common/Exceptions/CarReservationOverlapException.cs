using System.Net;

namespace CarRental.Common.Exceptions;

public class CarReservationOverlapException : CarRentalException
{
    public Guid CarId { get; }
    public DateTime RequestedPickupDate { get; }
    public DateTime RequestedReturnDate { get; }
    
    public CarReservationOverlapException(Guid carId, DateTime requestedPickupDate, DateTime requestedReturnDate) : base($"The dates for the car with ID {carId} overlap with an existing reservation. Requested pickup: {requestedPickupDate}, requested return: {requestedReturnDate}.")
    {
        CarId = carId;
        RequestedPickupDate = requestedPickupDate;
        RequestedReturnDate = requestedReturnDate;
    }

    public override HttpStatusCode HttpStatusCode => HttpStatusCode.Conflict;
}