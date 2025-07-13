using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Domain.Cards.ValueObjects;
using StudentCardAssignment.Domain.Students.ValueObjects;

namespace StudentCardAssignment.Application.Cards.Commands.AssignCard;

internal class AssignCardToStudentCommandHandler(
    IEventSourcedCardRepository cardRepository,
    IEventSourcedStudentRepository studentRepository) : ICommandHandler<AssignCardToStudentCommand, AssignCardToStudentResult>
{
    private readonly IEventSourcedCardRepository _cardRepository = cardRepository;
    private readonly IEventSourcedStudentRepository _studentRepository = studentRepository;

    public async Task<AssignCardToStudentResult> Handle(AssignCardToStudentCommand request, CancellationToken cancellationToken)
    {
        // Load Card aggregate from event store
        var card = await _cardRepository.GetByIdAsync(
            CardId.Create(request.CardId), cancellationToken) ?? throw new InvalidOperationException($"Card with ID '{request.CardId}' not found");

        // Load Student aggregate from event store
        var student = await _studentRepository.GetByIdAsync(
            StudentId.Create(request.StudentId), cancellationToken) ?? throw new InvalidOperationException($"Student with ID '{request.StudentId}' not found");

        if (!student.IsActive)
        {
            throw new InvalidOperationException("Cannot assign card to inactive student");
        }

        // Check if student already has a card assigned
        var existingCard = await _cardRepository.GetByAssignedStudentIdAsync(
            student.StudentId, cancellationToken);

        if (existingCard is not null)
        {
            throw new InvalidOperationException($"Student '{student.GetFullName()}' already has a card assigned");
        }

        // Execute the business logic - this will raise domain events
        card.AssignToStudent(student.StudentId);

        // Save the aggregate to the event store - this will persist the new events
        await _cardRepository.SaveAsync(card, cancellationToken);

        return new AssignCardToStudentResult(
            card.CardId.Value,
            student.StudentId.Value,
            card.CurrentAssignment!.AssignedAt);
    }
}
