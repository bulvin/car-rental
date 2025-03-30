namespace CarRental.Features.Cars.GetAvailabilityCars;

public sealed record AvailabilityCarResponse(Guid Id, string Model, string LocationName, decimal DailyRate);