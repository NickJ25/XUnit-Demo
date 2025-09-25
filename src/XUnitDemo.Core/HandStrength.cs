/// <summary>
/// Calculates the current hands strength
/// </summary>
/// <param name="Category"></param>
/// <param name="Tiebreakers">Ordered High to Low tiebreakers, eg. ranks of quads/ pair/ kickers</param>
public readonly record struct HandStrength(HandCategory Category, int[] Tiebreakers) : IComparable<HandStrength>
{
    public int CompareTo(HandStrength other)
    {
        int category = Category.CompareTo(other.Category);
        if (category != 0) return category;
        for (int i = 0; i < Math.Min(Tiebreakers.Length, other.Tiebreakers.Length); i++)
        {
            int cmp = Tiebreakers[i].CompareTo(other.Tiebreakers[i]);
            if (cmp != 0) return cmp;
        }
        return Tiebreakers.Length.CompareTo(other.Tiebreakers.Length);
    }

    public override string ToString()
    {
        return $"{Category}";
    }
}