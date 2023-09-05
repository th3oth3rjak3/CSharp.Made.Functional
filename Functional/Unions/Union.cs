using System.Diagnostics.CodeAnalysis;

namespace Functional.Unions;
public sealed class Union<A, B>
{
    private A Item1 { get; init; } = default!;
    private B Item2 { get; init; } = default!;
    readonly int tag;

    public Union(A item) { Item1 = item; tag = 1; }
    public Union(B item) { Item2 = item; tag = 2; }

    [ExcludeFromCodeCoverage]
    public T Match<T>(Func<A, T> caseOne, Func<B, T> casetwo) =>
        tag switch
        {
            1 => caseOne(Item1),
            2 => casetwo(Item2),
            _ => throw new NotImplementedException()
        };
}