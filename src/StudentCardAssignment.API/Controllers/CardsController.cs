using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentCardAssignment.Application.Cards.Commands.CreateCard;
using StudentCardAssignment.Application.Cards.Commands.AssignCard;
using StudentCardAssignment.Application.Cards.Commands.UnassignCard;
using StudentCardAssignment.Application.Cards.Queries.GetAllCards;
using StudentCardAssignment.Domain.Cards.Enums;

namespace StudentCardAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllCards(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCardsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateCardResult>> CreateCard(
        [FromBody] CreateCardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCardCommand(request.CardType, request.ExpiresAt);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCard), new { id = result.CardId }, result);
    }

    [HttpPost("{cardId:guid}/assign")]
    public async Task<ActionResult<AssignCardToStudentResult>> AssignCardToStudent(
        Guid cardId,
        [FromBody] AssignCardRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignCardToStudentCommand(cardId, request.StudentId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{cardId:guid}/unassign")]
    public async Task<ActionResult<UnassignCardResult>> UnassignCard(
        Guid cardId,
        CancellationToken cancellationToken)
    {
        var command = new UnassignCardCommand(cardId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public ActionResult GetCard(Guid id)
    {
        // TODO: Implement GetCard query using read model
        return Ok(new { Id = id, Message = "Card endpoint - implementation pending" });
    }
}

public record CreateCardRequest(CardType CardType, DateTime ExpiresAt);

public record AssignCardRequest(Guid StudentId);
