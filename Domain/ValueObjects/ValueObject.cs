using System.Linq;

namespace Domain.ValueObjects;

public abstract class ValueObject
{
    // VO içindeki property'leri karşılaştırmak için soyut metot
    protected abstract IEnumerable<object> GetEqualityComponents();

    // Equals override
    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != GetType()) 
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    // HashCode override
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(default(int), HashCode.Combine);
    }
}
