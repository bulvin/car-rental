using CarRental.Common;
using FluentValidation;

namespace CarRental.Features.Reservations.CreateReservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationCommand>
{
    private const int MaxRentalDays = 365;
    
    public CreateReservationValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("CarId is required.");

        RuleFor(x => x.PickupLocationId)
            .NotEmpty().WithMessage("PickupLocationId is required.");

        RuleFor(x => x.ReturnLocationId)
            .NotEmpty().WithMessage("ReturnLocationId is required.");

        RuleFor(x => x.PickupDate)
            .GreaterThan(DateTime.UtcNow.AddHours(1).RoundToFullHour()).WithMessage("Pickup must be at least one hour from now.")
            .Must(BeFullHour).WithMessage("Pickup date must be rounded to full hour.");

        RuleFor(x => x.ReturnDate)
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