namespace CarRental.Data.Entities;

public class Car : IEntity
{
    public Guid Id { get; set; }
    public CarModel Model { get; set; } = null!;
    public string Color { get; set; } = null!;
    public uint Year { get; set; }
    public decimal DailyRate { get; set; }
    public Guid CurrentLocationId { get; set; }
    public Location CurrentLocation { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;
}

public record CarModel(string Name, int PassengerCapacity)
{
    public static readonly CarModel ModelS = new("Model S", 5);
    public static readonly CarModel Model3 = new("Model 3", 5);
    public static readonly CarModel ModelX = new("Model X", 7);
    public static readonly CarModel ModelY = new("Model Y", 5);
}
