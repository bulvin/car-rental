using CarRental.Common;
using FluentValidation;

namespace CarRental.Features.Reservations.CalculateTotalCost;

public class CalculateTotalCostValidator : AbstractValidator<CalculateTotalCostQuery>
{
    private const int MaxRentalDays = 365;
    public CalculateTotalCostValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("Car id is required.");
        
        RuleFor(x => x.PickupDate)
            .NotEmpty().WithMessage("Pickup date is required.")
            .GreaterThan(DateTime.UtcNow.AddHours(1).RoundToFullHour()).WithMessage("Pickup must be at least one hour from now.")
            .Must(BeFullHour).WithMessage("Pickup date must be rounded to full hour.");

        RuleFor(x => x.ReturnDate)
            .NotEmpty().WithMessage("Return date is required.")
            .GreaterThan(x => x.PickupDate).WithMessage("Return date must be after pickup date.")
            .Must(BeFullHour).WithMessage("Return date must be rounded to full hour.")
            .Must((cmd, returnDate) => (returnDate - cmd.PickupDate).TotalDays <= MaxRentalDays)
            .WithMessage($"Maximum rental period is {MaxRentalDays} days.");
    }
    private static bool BeFullHour(DateTime date)
    {
        return date is { Minute: 0, Second: 0 };
    }
}