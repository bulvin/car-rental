using CarRental.Features.Reservations.CalculateTotalCost;
using CarRental.Features.Reservations.CreateReservation;
using CarRental.Features.Reservations.GetReservationDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarRental.Controllers;

[Route("/api/reservations")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a car reservation",
        Description = "Creates a new reservation and returns the reservation")]
    [SwaggerResponse(201, "Reservation created successfully", typeof(object))]
    [SwaggerResponse(400, "Invalid request data")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> CreateReservation(CreateReservationCommand reservation)
    {
        var reservationId = await _mediator.Send(reservation);
        return CreatedAtAction(nameof(GetCarReservation), new { id = reservationId }, reservation);
    }

    [HttpGet("cost")]
    [SwaggerOperation(
        Summary = "Calculate reservation cost",
        Description = "Calculates and returns the total cost for a given reservation based on provided parameters.")]
    [SwaggerResponse(200, "Total reservation cost", typeof(ReservationCostResponse))]
    [SwaggerResponse(400, "Invalid request data")]
    [ProducesResponseType(typeof(decimal), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> CalculateReservationCost([FromQuery] CalculateTotalCostQuery query)
    {
        var cost = await _mediator.Send(query);
        return Ok(cost);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get reservation details",
        Description = "Fetches details of a specific reservation using its unique ID.")]
    [SwaggerResponse(200, "Reservation details retrieved", typeof(ReservationDetailsResponse))]
    [SwaggerResponse(404, "Reservation not found")]
    [ProducesResponseType(typeof(ReservationDetailsResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> GetCarReservation([FromRoute] string id)
    {
        var reservation = await _mediator.Send(new GetReservationDetailsQuery(id));
        return Ok(reservation);
    }
}