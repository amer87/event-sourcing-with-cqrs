using MediatR;

namespace StudentCardAssignment.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}
