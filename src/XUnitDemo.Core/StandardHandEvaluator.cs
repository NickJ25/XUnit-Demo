public sealed class StandardHandEvaluator : IHandEvaluator
{
    public HandStrength Evaluate(Hand hand)
    {
        var cards = hand.Cards;

        // Histograms
        var ranks = cards.Select(c => (int)c.Rank).OrderByDescending(r => r).ToArray();
        var suits = cards.GroupBy(c => c.Suit).ToDictionary(g => g.Key, g => g.Count());
        var rankGroups = cards.GroupBy(c => c.Rank)
            .Select(g => new { Rank = (int)g.Key, Count = g.Count() })
            .OrderByDescending(rc => rc.Count)
            .ThenByDescending(rc => rc.Rank)
            .ToList();

        bool isFlush = suits.Values.Any(cards => cards == 5);
        bool isStraight = IsStraight(ranks, out int highStraight);

        // Royal Flush
        if (isFlush && isStraight && highStraight == (int)Rank.Ace)
            return new HandStrength(HandCategory.RoyalFlush, new[] { highStraight });

        // Straight Flush
        if (isFlush && isStraight)
            return new HandStrength(HandCategory.StraightFlush, new[] { highStraight });

        // Four of a Kind
        if (rankGroups[0].Count == 4)
        {
            var quad = rankGroups[0].Rank;
            var kicker = rankGroups[1].Rank;
            return new HandStrength(HandCategory.FourOfAKind, new[] { quad, kicker });
        }

        // Full House
        if (rankGroups[0].Count == 3 && rankGroups[1].Count == 2)
            return new HandStrength(HandCategory.FullHouse, new[] { rankGroups[0].Rank, rankGroups[1].Rank });

        // Flush
        if (isFlush)
            return new HandStrength(HandCategory.Flush, ranks);

        // Straight
        if (isStraight)
            return new HandStrength(HandCategory.Straight, ranks);

        // Three of a Kind
        if (rankGroups[0].Count == 3)
        {
            var trip = rankGroups[0].Rank;
            var kickers = rankGroups.Skip(1).Select(g => g.Rank).OrderByDescending(r => r).ToArray();
            return new HandStrength(HandCategory.ThreeOfAKind, new[] { trip }.Concat(kickers).ToArray());
        }

        // Two Pair
        if (rankGroups[0].Count == 2 && rankGroups[1].Count == 2)
        {
            var highPair = Math.Max(rankGroups[0].Rank, rankGroups[1].Rank);
            var lowPair = Math.Min(rankGroups[0].Rank, rankGroups[1].Rank);
            var kicker = rankGroups[2].Rank;
            return new HandStrength(HandCategory.TwoPair, new[] { highPair, lowPair, kicker });
        }

        // Pair
        if (rankGroups[0].Count == 2)
        {
            var pair = rankGroups[0].Rank;
            var kickers = rankGroups.Skip(1).SelectMany(g => Enumerable.Repeat(g.Rank, g.Count)).OrderByDescending(r => r).ToArray();
            return new HandStrength(HandCategory.OnePair, new[] { pair }.Concat(kickers).ToArray());
        }

        // High Card
        return new HandStrength(HandCategory.HighCard, ranks);
    }

    private static bool IsStraight(int[] sortedDescRanks, out int highRank)
    {
        var distinct = sortedDescRanks.Distinct().OrderByDescending(r => r).ToArray();
        if (distinct.Length < 5) { highRank = 0; return false; }

        int run = 1; highRank = distinct[0];
        for (int i = 1; i < distinct.Length; i++)
        {
            if (distinct[i] == distinct[i - 1] - 1)
            {
                run++;
                if (run == 5) { highRank = distinct[i - 4 + 4]; return true; }
            }
            else run = 1;
        }

        // Check for 5-4-3-2-A
        var set = distinct.ToHashSet();
        if (set.Contains((int)Rank.Ace) && set.Contains(2) && set.Contains(3) && set.Contains(4) && set.Contains(5))
        {
            highRank = 5;
            return true;
        }

        highRank = 0;
        return false;
    }
}