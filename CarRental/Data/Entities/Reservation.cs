namespace CarRental.Data.Entities;

public class Reservation : IEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public Guid CarId { get; set; }
    public Car Car { get; set; } = null!;
    public Guid PickupLocationId { get; set; }
    public Location PickupLocation { get; set; } = null!;
    public Guid ReturnLocationId { get; set; }
    public Location ReturnLocation { get; set; } = null!;
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal TotalCost { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
}

public enum ReservationStatus
{
    Reserved,
    Completed,
    Cancelled
}