using CarRental.Features.Reservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> CreateReservation(CreateReservationCommand reservation)
    {
        var reservationId = await _mediator.Send(reservation);
        return Created($"/api/reservations/{reservationId}", new { id = reservationId });
    }
}