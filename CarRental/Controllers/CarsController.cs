using CarRental.Features.Cars.GetAvailabilityCars;
using CarRental.Features.Cars.GetCarDetails;
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

    [HttpGet("{id:guid}")]
    public async Task<Ok<CarDetailsResponse>> GetCarDetails(Guid id)
    {
        var carDetails = await _mediator.Send(new GetCarDetailsQuery(id));
        return TypedResults.Ok(carDetails);
    }
}