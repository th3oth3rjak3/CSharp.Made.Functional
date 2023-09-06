using System.Diagnostics.CodeAnalysis;

namespace Functional.Unions;

public sealed class Union4<A, B, C, D>
{
    private A Item1 { get; init; } = default!;
    private B Item2 { get; init; } = default!;
    private C Item3 { get; init; } = default!;
    private D Item4 { get; init; } = default!;

    private readonly int tag;

    public Union4(A item) { Item1 = item; tag = 1; }
    public Union4(B item) { Item2 = item; tag = 2; }
    public Union4(C item) { Item3 = item; tag = 3; }
    public Union4(D item) { Item4 = item; tag = 4; }

    [ExcludeFromCodeCoverage]
    public T Match<T>(Func<A, T> caseOne, Func<B, T> caseTwo, Func<C, T> caseThree, Func<D, T> caseFour) =>
        tag switch
        {
            1 => caseOne(Item1),
            2 => caseTwo(Item2),
            3 => caseThree(Item3),
            4 => caseFour(Item4),
            _ => throw new NotImplementedException()
        };
}
