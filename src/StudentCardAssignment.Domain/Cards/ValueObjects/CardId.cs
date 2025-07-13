using StudentCardAssignment.Domain.Common;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Cards.ValueObjects;

public class CardId : ValueObject
{
    public Guid Value { get; }

    [JsonConstructor]
    private CardId(Guid value)
    {
        Value = value;
    }

    public static CardId CreateUnique()
    {
        return new CardId(Guid.NewGuid());
    }

    public static CardId Create(Guid value)
    {
        return new CardId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
