namespace Functional;

/// <summary>
/// Represents a Discriminated Union which contains two Variants.
/// </summary>
/// <typeparam name="A">The type of the first variant.</typeparam>
/// <typeparam name="B">The type of the second variant.</typeparam>
public sealed record Union<A, B>
{
    /// <summary>
    /// The inner state of the Union.
    /// </summary>
    private enum State
    {
        A,
        B
    }

    /// <summary>
    /// The inner value when the State is A.
    /// </summary>
    private readonly A _a = default!;

    /// <summary>
    /// The inner value when the State is B.
    /// </summary>
    private readonly B _b = default!;

    /// <summary>
    /// The state of the Union. Either A or B.
    /// </summary>
    private readonly State _state;

    /// <summary>
    /// Construct a new Union from <typeparamref name="A"/>
    /// </summary>
    /// <param name="a">The value of the Union.</param>
    public Union(A a)
    {
        _a = a;
        _state = State.A;
    }

    /// <summary>
    /// Construct a new Union from <typeparamref name="B"/>
    /// </summary>
    /// <param name="b">The value of the Union.</param>
    public Union(B b)
    {
        _b = b;
        _state = State.B;
    }

    /// <summary>
    /// Match the inner value of one of the variants and perform a mapping function.
    /// </summary>
    /// <typeparam name="T">The mapping output type.</typeparam>
    /// <param name="caseOne">The function to execute if the type is <typeparamref name="A"/></param>
    /// <param name="caseTwo">The function to execute if the type is <typeparamref name="B"/></param>
    /// <returns>The result of the mapping function that was executed.</returns>
    public T Match<T>(Func<A, T> caseOne, Func<B, T> caseTwo)
    {
        if (_state == State.A) return caseOne(_a);
        return caseTwo(_b);
    }

    /// <summary>
    /// Perform a side-effect on the Union based on its inner type.
    /// </summary>
    /// <param name="caseOne">The action to perform when the Union is a <typeparamref name="A"/>.</param>
    /// <param name="caseTwo">The action to perform when the Union is a <typeparamref name="B"/>.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<A> caseOne, Action<B> caseTwo)
    {
        if (_state == State.A) caseOne(_a);
        if (_state == State.B) caseTwo(_b);

        return new();
    }
}