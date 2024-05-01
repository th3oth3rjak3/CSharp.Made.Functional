namespace Functional.Unions;

/// <summary>
/// Represents a Discriminated Union which contains two Variants.
/// </summary>
/// <typeparam name="A">The type of the first variant.</typeparam>
/// <typeparam name="B">The type of the second variant.</typeparam>
public sealed class Union<A, B>
{
    private A Item1 { get; init; } = default!;
    private B Item2 { get; init; } = default!;

    private readonly int tag;

    /// <summary>
    /// Construct a new Union from <typeparamref name="A"/>
    /// </summary>
    /// <param name="item">The value of the Union.</param>
    public Union(A item) { Item1 = item; tag = 1; }

    /// <summary>
    /// Construct a new Union from <typeparamref name="B"/>
    /// </summary>
    /// <param name="item">The value of the Union.</param>
    public Union(B item) { Item2 = item; tag = 2; }

    /// <summary>
    /// Match the inner value of one of the variants and perform a mapping function.
    /// </summary>
    /// <typeparam name="T">The mapping output type.</typeparam>
    /// <param name="caseOne">The function to execute if the type is <typeparamref name="A"/></param>
    /// <param name="caseTwo">The function to execute if the type is <typeparamref name="B"/></param>
    /// <returns>The result of the mapping function that was executed.</returns>
    [ExcludeFromCodeCoverage]
    public T Match<T>(Func<A, T> caseOne, Func<B, T> caseTwo) =>
        tag switch
        {
            1 => caseOne(Item1),
            2 => caseTwo(Item2),
            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Match the inner value of one of the variants and perform an action based on its type.
    /// </summary>
    /// <param name="caseOne">The action to execute when the inner type is <typeparamref name="A"/></param>
    /// <param name="caseTwo">The action to execute when the inner type is <typeparamref name="B"/></param>
    public Unit Effect(Action<A> caseOne, Action<B> caseTwo)
    {
        switch (tag)
        {
            case 1:
                caseOne(Item1);
                break;
            case 2:
                caseTwo(Item2);
                break;
        }

        return Unit.Default;
    }

}