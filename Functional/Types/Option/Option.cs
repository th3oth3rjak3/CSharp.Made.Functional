namespace Functional;

// TODO: Document this file with examples.

/// <summary>
/// A wrapper class which contains either Some value or no value (None).
/// </summary>
/// <typeparam name="T">The inner type of the Option.</typeparam>
public sealed record Option<T>
{

    /// <summary>
    /// Represents the internal state of the Option.
    /// </summary>
    private enum State
    {
        Some,
        None
    }

    /// <summary>
    /// The internal contents of the Option.
    /// </summary>
    private readonly T? _contents;

    /// <summary>
    /// The current state of the Option. None by default.
    /// </summary>
    private readonly State _state = State.None;

    /// <summary>
    /// Create a new option.
    /// </summary>
    /// <param name="input">Input which may be null.</param>
    public Option(T? input)
    {
        if (input is not null)
        {
            _contents = input;
            _state = State.Some;
        }
    }

    /// <summary>
    /// Create a new option that is a None.
    /// </summary>
    public Option() { }

    /// <summary>
    /// Determine if an Option is Some.
    /// </summary>
    public bool IsSome => _state == State.Some;

    /// <summary>
    /// Determine if an Option is None.
    /// </summary>
    public bool IsNone => _state == State.None;

    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. If an option is None, it will throw an InvalidOperationException.
    /// <see cref="IsSome"/> or <see cref="IsNone"/>.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will be fine
    /// new Option&lt;string&gt;("hello, world!").Unwrap();
    /// 
    /// // This will throw an InvalidOperationException
    /// new Option&lt;string&gt;(null as string).Unwrap();
    /// 
    /// // To use this safely, perform the following steps
    /// Option&lt;string&gt; optional = new Option&lt;string&gt;(null as string); // This will be a None.
    /// 
    /// if (optional.IsSome)
    /// {
    ///     var contents = optional.Unwrap();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping a None.</exception>
    public T Unwrap()
    {
        if (this.IsSome && this._contents is not null) return _contents;
        throw new InvalidOperationException("Tried to unwrap a None! Be sure to check with IsSome before unwrapping.");
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSome, Func<TResult> whenNone) =>
        IsSome.Match(
            () => whenSome(Unwrap()),
            whenNone);

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action<T> doWhenSome, Action doWhenNone)
    {
        if (IsSome) doWhenSome(Unwrap());
        if (IsNone) doWhenNone();

        return new();
    }

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action doWhenSome, Action doWhenNone)
    {
        if (IsSome) doWhenSome();
        if (IsNone) doWhenNone();
        return new();
    }

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public Unit EffectSome(Action<T> doWhenSome)
    {
        if (IsSome) doWhenSome(Unwrap());
        return new();
    }


    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit EffectNone(Action doWhenNone)
    {
        if (IsNone) doWhenNone();
        return new();
    }

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action<T> whenSome, Action whenNone)
    {
        if (IsSome) whenSome(Unwrap());
        if (IsNone) whenNone();

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action whenSome, Action whenNone)
    {
        if (IsSome) whenSome();
        if (IsNone) whenNone();

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(params Action<T>[] whenSome)
    {
        if (IsSome)
        {
            var contents = Unwrap();
            whenSome.ToList().ForEach(action => action(contents));
        }

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(params Action[] whenSome)
    {
        if (IsSome)
        {
            whenSome.ToList().ForEach(action => action());
        }

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is None.
    /// </summary>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapNone(params Action[] whenNone)
    {
        if (IsNone)
        {
            whenNone.ToList().ForEach(action => action());
        }

        return this;
    }
}

/// <summary>
/// A wrapper class which contains either Some value or no value (None).
/// </summary>
public static class Option
{
    /// <summary>
    /// Create an Option that represents some value.
    /// </summary>
    /// <typeparam name="T">The type of the inner content.</typeparam>
    /// <param name="entity">The contents to store.</param>
    /// <returns>A new Option with some data inside.</returns>
    public static Option<T> Some<T>(this T entity) where T : notnull =>
        new(entity);


    /// <summary>
    /// Create an Option that represents no value.
    /// </summary>
    /// <typeparam name="T">The type of the contents if they had been present.</typeparam>
    /// <returns>A new Option that represents a lack of contents.</returns>
    public static Option<T> None<T>() =>
        new();
}