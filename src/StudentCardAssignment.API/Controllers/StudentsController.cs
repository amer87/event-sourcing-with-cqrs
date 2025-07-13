using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentCardAssignment.Application.Students.Commands.CreateStudent;
using StudentCardAssignment.Application.Students.Queries.GetStudent;
using StudentCardAssignment.Application.Students.Queries.GetAllStudents;

namespace StudentCardAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllStudents(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllStudentsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateStudentResult>> CreateStudent(
        [FromBody] CreateStudentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateStudentCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.StudentNumber);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetStudent), new { id = result.StudentId }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetStudentResult>> GetStudent(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}

public record CreateStudentRequest(
    string FirstName,
    string LastName,
    string Email,
    string StudentNumber);
