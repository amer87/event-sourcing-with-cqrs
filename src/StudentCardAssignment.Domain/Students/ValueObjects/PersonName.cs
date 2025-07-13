using StudentCardAssignment.Domain.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Students.ValueObjects;

public class PersonName : ValueObject
{
    public string Value { get; }

    [JsonConstructor]
    private PersonName(string value)
    {
        Value = value;
    }

    public static PersonName Create(string name)
    {
        ValidateName(name);
        var normalizedName = NormalizeName(name);
        return new PersonName(normalizedName);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }

        if (name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters", nameof(name));
        }

        if (name.Length > 50)
        {
            throw new ArgumentException("Name cannot exceed 50 characters", nameof(name));
        }

        // Allow letters, spaces, hyphens, apostrophes, and basic international characters
        if (!Regex.IsMatch(name, @"^[\p{L}\s\-'\.]+$"))
        {
            throw new ArgumentException("Name contains invalid characters", nameof(name));
        }
    }

    private static string NormalizeName(string name)
    {
        // Trim whitespace and normalize to title case
        var trimmed = name.Trim();

        // Handle multiple spaces
        var singleSpaced = Regex.Replace(trimmed, @"\s+", " ");

        // Convert to title case (first letter of each word capitalized)
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(singleSpaced.ToLower());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is string str)
        {
            return Value == str;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    public static implicit operator string(PersonName name) => name.Value;

    public override string ToString() => Value;
}
