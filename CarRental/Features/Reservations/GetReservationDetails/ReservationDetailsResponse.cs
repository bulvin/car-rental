using CarRental.Data.Entities;

namespace CarRental.Features.Reservations.GetReservationDetails;

public class ReservationDetailsResponse
{
    public Guid Id { get; init; }
    public string ReservationNumber { get; init; }
    public CarReservationDetailsResponse Car { get; init; }
    public string PickupLocationName { get; init; }
    public string ReturnLocationName { get; init; }
    public DateTime PickupDate { get; init; }
    public DateTime ReturnDate { get; init; }
    public decimal TotalCost { get; init; }
    public ReservationStatus Status { get; init; }
}

public record CarReservationDetailsResponse(Guid CarId, string Model, string Color);