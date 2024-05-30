namespace Functional;

/// <summary>
/// Represents a Discriminated Union which contains six Variants.
/// </summary>
/// <typeparam name="A">The type of the first variant.</typeparam>
/// <typeparam name="B">The type of the second variant.</typeparam>
/// <typeparam name="C">The type of the third variant.</typeparam>
/// <typeparam name="D">The type of the fourth variant.</typeparam>
/// <typeparam name="E">The type of the fifth variant.</typeparam>
/// <typeparam name="F">The type of the sixth variant.</typeparam>
public sealed record Union<A, B, C, D, E, F>
{
    /// <summary>
    /// The inner state of the Union.
    /// </summary>
    private enum State
    {
        A,
        B,
        C,
        D,
        E,
        F
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
    /// The inner value when the State is C.
    /// </summary>
    private readonly C _c = default!;

    /// <summary>
    /// The inner value when the State is D.
    /// </summary>
    private readonly D _d = default!;

    /// <summary>
    /// The inner value when the State is E.
    /// </summary>
    private readonly E _e = default!;

    /// <summary>
    /// The inner value when the State is F.
    /// </summary>
    private readonly F _f = default!;

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
    /// Construct a new Union from <typeparamref name="C"/>
    /// </summary>
    /// <param name="c">The value of the Union.</param>
    public Union(C c)
    {
        _c = c;
        _state = State.C;
    }

    /// <summary>
    /// Construct a new Union from <typeparamref name="D"/>
    /// </summary>
    /// <param name="d">The value of the Union.</param>
    public Union(D d)
    {
        _d = d;
        _state = State.D;
    }

    /// <summary>
    /// Construct a new Union from <typeparamref name="E"/>
    /// </summary>
    /// <param name="e">The value of the Union.</param>
    public Union(E e)
    {
        _e = e;
        _state = State.E;
    }

    /// <summary>
    /// Construct a new Union from <typeparamref name="F"/>
    /// </summary>
    /// <param name="f">The value of the Union.</param>
    public Union(F f)
    {
        _f = f;
        _state = State.F;
    }

    /// <summary>
    /// Match the inner value of one of the variants and perform a mapping function.
    /// </summary>
    /// <typeparam name="T">The mapping output type.</typeparam>
    /// <param name="whenA">The function to execute if the type is <typeparamref name="A"/></param>
    /// <param name="whenB">The function to execute if the type is <typeparamref name="B"/></param>
    /// <param name="whenC">The function to execute if the type is <typeparamref name="C"/></param>
    /// <param name="whenD">The function to execute if the type is <typeparamref name="D"/></param>
    /// <param name="whenE">The function to execute if the type is <typeparamref name="E"/></param>
    /// <param name="whenF">The function to execute if the type is <typeparamref name="F"/></param>
    /// <returns>The result of the mapping function that was executed.</returns>
    public T Match<T>(Func<A, T> whenA, Func<B, T> whenB, Func<C, T> whenC, Func<D, T> whenD, Func<E, T> whenE, Func<F, T> whenF)
    {
        if (_state == State.A) return whenA(_a);
        if (_state == State.B) return whenB(_b);
        if (_state == State.C) return whenC(_c);
        if (_state == State.D) return whenD(_d);
        if (_state == State.E) return whenE(_e);
        return whenF(_f);
    }

    /// <summary>
    /// Perform a side effect on the Union based on its inner type.
    /// </summary>
    /// <param name="whenA">The action to perform when the Union is a <typeparamref name="A"/>.</param>
    /// <param name="whenB">The action to perform when the Union is a <typeparamref name="B"/>.</param>
    /// <param name="whenC">The action to perform when the Union is a <typeparamref name="C"/>.</param>
    /// <param name="whenD">The action to perform when the Union is a <typeparamref name="D"/>.</param>
    /// <param name="whenE">The action to perform when the Union is a <typeparamref name="E"/>.</param>
    /// <param name="whenF">The action to perform when the Union is a <typeparamref name="F"/>.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<A> whenA, Action<B> whenB, Action<C> whenC, Action<D> whenD, Action<E> whenE, Action<F> whenF)
    {
        if (_state == State.A) whenA(_a);
        if (_state == State.B) whenB(_b);
        if (_state == State.C) whenC(_c);
        if (_state == State.D) whenD(_d);
        if (_state == State.E) whenE(_e);
        if (_state == State.F) whenF(_f);

        return new();
    }
}