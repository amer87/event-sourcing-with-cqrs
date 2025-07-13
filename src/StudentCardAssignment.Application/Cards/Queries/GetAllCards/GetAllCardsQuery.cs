using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Cards.Queries.Common;

namespace StudentCardAssignment.Application.Cards.Queries.GetAllCards;

public record GetAllCardsQuery : IQuery<IEnumerable<CardReadModel>>;
