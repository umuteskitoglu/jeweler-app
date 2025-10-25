namespace Domain.ValueObjects;

public class AuditInfo : ValueObject
{
    public AuditInfo() { }
    
    public AuditInfo(DateTime? at, Guid by)
    {
        At = at;
        By = by;
    }

    public DateTime? At { get; set; }
    public Guid By { get; set; }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return At;
        yield return By;
    }

    public override bool Equals(object obj)
    {
        if (obj is AuditInfo other)
        {
            return At == other.At && By == other.By;
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(At, By);
}