namespace CarRental.Data.Entities;

public class Car : IEntity
{
    public Guid Id { get; set; }
    public string Model { get; set; } = null!;
    public string Color { get; set; } = null!;
    public uint Year { get; set; }
    public decimal DailyRate { get; set; }
    public Guid CurrentLocationId { get; set; }
    public Location CurrentLocation { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;
}
