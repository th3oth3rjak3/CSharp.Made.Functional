using System.Diagnostics.CodeAnalysis;

namespace Functional.Unions;

public sealed class Union5<A, B, C, D, E>
{
    private A Item1 { get; init; } = default!;
    private B Item2 { get; init; } = default!;
    private C Item3 { get; init; } = default!;
    private D Item4 { get; init; } = default!;
    private E Item5 { get; init; } = default!;

    private readonly int tag;

    public Union5(A item) { Item1 = item; tag = 1; }
    public Union5(B item) { Item2 = item; tag = 2; }
    public Union5(C item) { Item3 = item; tag = 3; }
    public Union5(D item) { Item4 = item; tag = 4; }
    public Union5(E item) { Item5 = item; tag = 5; }

    [ExcludeFromCodeCoverage]
    public T Match<T>(Func<A, T> caseOne, Func<B, T> caseTwo, Func<C, T> caseThree, Func<D, T> caseFour, Func<E, T> caseFive) =>
        tag switch
        {
            1 => caseOne(Item1),
            2 => caseTwo(Item2),
            3 => caseThree(Item3),
            4 => caseFour(Item4),
            5 => caseFive(Item5),
            _ => throw new NotImplementedException()
        };
}