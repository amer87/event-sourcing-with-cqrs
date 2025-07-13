using StudentCardAssignment.Application.Common;
using StudentCardAssignment.Application.Common.Interfaces;
using StudentCardAssignment.Application.Cards.Queries.Common;

namespace StudentCardAssignment.Application.Cards.Queries.GetAllCards;

internal class GetAllCardsQueryHandler : IQueryHandler<GetAllCardsQuery, IEnumerable<CardReadModel>>
{
    private readonly ICardReadModelRepository _cardReadModelRepository;

    public GetAllCardsQueryHandler(ICardReadModelRepository cardReadModelRepository)
    {
        _cardReadModelRepository = cardReadModelRepository;
    }

    public async Task<IEnumerable<CardReadModel>> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
    {
        return await _cardReadModelRepository.GetAllAsync(cancellationToken);
    }
}
