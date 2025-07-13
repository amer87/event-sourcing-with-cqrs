using StudentCardAssignment.Domain.Common;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Students.ValueObjects;

public class StudentNumber : ValueObject
{
    public string Value { get; }

    [JsonConstructor]
    private StudentNumber(string value)
    {
        Value = value;
    }

    public static StudentNumber Create(string studentNumber)
    {
        if (string.IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number cannot be null or empty");

        if (studentNumber.Length < 5 || studentNumber.Length > 20)
            throw new ArgumentException("Student number must be between 5 and 20 characters");

        return new StudentNumber(studentNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
