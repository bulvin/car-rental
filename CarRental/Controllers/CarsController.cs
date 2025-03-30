using CarRental.Features.Cars.GetAvailabilityCars;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

[Route("/api/cars")]
[ApiController]
public class CarsController
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Ok<List<AvailabilityCarResponse>>> GetAvailableCars([FromQuery] GetAvailabilityCarsQuery query)
    {
        var availableCars = await _mediator.Send(query);
        return TypedResults.Ok(availableCars);
    }
}