using CarRental.Features.Cars.GetAvailabilityCars;
using CarRental.Features.Cars.GetCarDetails;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Get available cars",
        Description = "Returns a list of cars that match the specified availability criteria.")]
    [SwaggerResponse(200, "List of available cars", typeof(List<AvailabilityCarResponse>))]
    [SwaggerResponse(400, "Bad request")]
    [ProducesResponseType(typeof(List<AvailabilityCarResponse>), 200)]
    [ProducesResponseType(400)]
    public async Task<Ok<List<AvailabilityCarResponse>>> GetAvailableCars([FromQuery] GetAvailabilityCarsQuery query)
    {
        var availableCars = await _mediator.Send(query);
        return TypedResults.Ok(availableCars);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get car details by id",
        Description = "Returns information about a specific car.")]
    [SwaggerResponse(200, "Car details", typeof(CarDetailsResponse))]
    [SwaggerResponse(404, "Car not found")]
    [ProducesResponseType(typeof(CarDetailsResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<Ok<CarDetailsResponse>> GetCarDetails(Guid id)
    {
        var carDetails = await _mediator.Send(new GetCarDetailsQuery(id));
        return TypedResults.Ok(carDetails);
    }
}