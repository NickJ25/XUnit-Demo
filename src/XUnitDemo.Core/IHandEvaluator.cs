public interface IHandEvaluator
{
    HandStrength Evaluate(Hand hand);
    int Compare(Hand a, Hand b) => Evaluate(a).CompareTo(Evaluate(b));
}