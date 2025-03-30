using CarRental.Data.Entities;
using FluentValidation;

namespace CarRental.Features.Cars.GetAvailabilityCars;

public sealed class GetAvailabilityCarsValidator : AbstractValidator<GetAvailabilityCarsQuery>
{
    public GetAvailabilityCarsValidator()
    {
        RuleFor(q => q.StartDate)
            .LessThan(q => q.EndDate)
            .WithMessage("Start date must be earlier than end date");
        
        RuleFor(q => q.EndDate)
            .GreaterThan(q => q.StartDate)
            .WithMessage("End date must be later than start date");
        
        RuleFor(query => query.Model)
            .Must(model => model == null || IsValidTeslaPassengerModel(model)) 
            .WithMessage("Model must be one of the following Tesla passenger models: Model S, Model 3, Model X, Model Y.");
    }
    
    private static bool IsValidTeslaPassengerModel(string model)
    {
        string[] validTeslaModels = [CarModel.ModelS.Name, CarModel.Model3.Name, CarModel.ModelX.Name, CarModel.ModelY.Name];
        return validTeslaModels.Contains(model, StringComparer.OrdinalIgnoreCase);
    }
}