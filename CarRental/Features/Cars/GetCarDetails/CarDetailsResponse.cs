namespace CarRental.Features.Cars.GetCarDetails;

public record CarDetailsResponse(
    Guid Id, 
    string Model, 
    int PassengerCapacity, 
    string Color, 
    uint Year, 
    decimal DailyRate, 
    string LocationName);