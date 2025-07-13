using StudentCardAssignment.Domain.Common;
using System.Text.Json.Serialization;

namespace StudentCardAssignment.Domain.Students.ValueObjects;

public class StudentId : ValueObject
{
    public Guid Value { get; }

    [JsonConstructor]
    private StudentId(Guid value)
    {
        Value = value;
    }

    public static StudentId CreateUnique()
    {
        return new StudentId(Guid.NewGuid());
    }

    public static StudentId Create(Guid value)
    {
        return new StudentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
