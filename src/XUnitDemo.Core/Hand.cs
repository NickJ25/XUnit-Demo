public enum Suit { Clubs, Diamonds, Hearts, Spades }
public enum Rank : int
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

public readonly record struct Card(Rank Rank, Suit Suit);

public sealed class Hand
{
    public IReadOnlyList<Card> Cards { get; }
    public Hand(IEnumerable<Card> cards)
    {
        var list = cards?.ToList() ?? throw new ArgumentNullException(nameof(cards));
        if (list.Count != 5) throw new ArgumentException("Hand must have exactly five cards");
        if (list.Distinct().Count() != 5) throw new ArgumentException("Hand have five unique cards");
        Cards = list.AsReadOnly();
    }
}

