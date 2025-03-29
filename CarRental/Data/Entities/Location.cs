namespace CarRental.Data.Entities;

public class Location : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}