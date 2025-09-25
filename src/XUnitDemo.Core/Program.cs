var evaluator = new StandardHandEvaluator();

var handA = new Hand(new[]{
    new Card(Rank.Ace, Suit.Spades),
    new Card(Rank.Ace, Suit.Diamonds),
    new Card(Rank.Ace, Suit.Clubs),
    new Card(Rank.Ace, Suit.Hearts),
    new Card(Rank.Queen, Suit.Spades),
});

var handB = new Hand(new[]{
    new Card(Rank.Ace, Suit.Spades),
    new Card(Rank.Queen, Suit.Spades),
    new Card(Rank.Two, Suit.Spades),
    new Card(Rank.Five, Suit.Spades),
    new Card(Rank.King, Suit.Spades),
});

var sA = evaluator.Evaluate(handA);
var sB = evaluator.Evaluate(handB);

Console.WriteLine($"HandA: {sA.ToString()}");
Console.WriteLine($"HandB: {sB.ToString()}");

int compare = sA.CompareTo(sB);

Console.WriteLine(compare);