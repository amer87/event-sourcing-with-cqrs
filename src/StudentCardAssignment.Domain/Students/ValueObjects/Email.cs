using StudentCardAssignment.Domain.Common;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Students.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    [JsonConstructor]
    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        ValidateEmail(email);
        return new Email(email);
    }

    private static void ValidateEmail(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        if (email.Length > 256)
        {
            throw new ArgumentException("Email is too long", nameof(email));
        }
        var addr = new System.Net.Mail.MailAddress(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
