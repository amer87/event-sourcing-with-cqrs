using StudentCardAssignment.Domain.Common;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Cards.ValueObjects;

public class CardNumber : ValueObject
{
    public string Value { get; }

    [JsonConstructor]
    private CardNumber(string value)
    {
        Value = value;
    }

    public static CardNumber Create(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            throw new ArgumentException("Card number cannot be null or empty");

        if (cardNumber.Length != 16)
            throw new ArgumentException("Card number must be exactly 16 characters");

        if (!cardNumber.All(char.IsDigit))
            throw new ArgumentException("Card number must contain only digits");

        return new CardNumber(cardNumber);
    }

    public static CardNumber Generate()
    {
        var random = new Random();
        var cardNumber = string.Empty;

        for (int i = 0; i < 16; i++)
        {
            cardNumber += random.Next(0, 10).ToString();
        }

        return new CardNumber(cardNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public string GetMaskedNumber()
    {
        return $"****-****-****-{Value.Substring(12)}";
    }
}
