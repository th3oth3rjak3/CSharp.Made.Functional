using System.Diagnostics.CodeAnalysis;

namespace Functional.Unions;

public sealed class Union3<A, B, C>
{
    private A Item1 { get; init; } = default!;
    private B Item2 { get; init; } = default!;
    private C Item3 { get; init; } = default!;

    private readonly int tag;

    public Union3(A item) { Item1 = item; tag = 1; }
    public Union3(B item) { Item2 = item; tag = 2; }
    public Union3(C item) { Item3 = item; tag = 3; }

    [ExcludeFromCodeCoverage]
    public T Match<T>(Func<A, T> caseOne, Func<B, T> caseTwo, Func<C, T> caseThree) =>
        tag switch
        {
            1 => caseOne(Item1),
            2 => caseTwo(Item2),
            3 => caseThree(Item3),
            _ => throw new NotImplementedException()
        };
}
