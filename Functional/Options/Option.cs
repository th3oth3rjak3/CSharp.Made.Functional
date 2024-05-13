namespace Functional.Options;

/// <summary>
/// A wrapper class which contains either Some value or no value (None).
/// </summary>
/// <typeparam name="T">The inner type of the Option.</typeparam>
[ExcludeFromCodeCoverage]
public sealed record Option<T>
{
    private T? Contents { get; }
    private Union<Some<T>, None<T>> Union { get; }

    /// <summary>
    /// Construct a new Option from a Some.
    /// </summary>
    /// <param name="some">A Some object which contains value.</param>
    public Option(Some<T> some)
    {
        Contents = some.Contents;
        Union = new Union<Some<T>, None<T>>(some);
    }

    /// <summary>
    /// Construct a new Option from a None.
    /// </summary>
    /// <param name="none">A None object with no value.</param>
    public Option(None<T> none) => Union = new Union<Some<T>, None<T>>(none);

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSome, Func<TResult> whenNone) =>
        Union
            .Match(
                some => whenSome(some.Contents),
                _ => whenNone());

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action<T> doWhenSome, Action doWhenNone) =>
        Union
            .Effect(some => doWhenSome(some.Contents), _ => doWhenNone());

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action doWhenSome, Action doWhenNone) =>
        Union
            .Effect(_ => doWhenSome(), _ => doWhenNone());

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public Unit EffectSome(Action<T> doWhenSome) =>
        Union
            .Effect(
                some => doWhenSome(some.Contents),
                _ => { });

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit EffectNone(Action doWhenNone) =>
        Union
            .Effect(
                _ => { },
                _ => doWhenNone());

    /// <summary>
    /// Determine if the option contains some value. True when Some, otherwise false.
    /// </summary>
    public bool IsSome =>
        Union.Match(_ => true, _ => false);

    /// <summary>
    /// Determine if the option contains no value. True when None, otherwise false.
    /// </summary>
    public bool IsNone =>
        Union.Match(_ => false, _ => true);

    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. If an option is None, it will return the default value.
    /// <br /><br />
    /// This means that the value will be null for reference types or the standard default value for 
    /// primitive types. For example Option.None&lt;int&gt;.Unwrap() will return 0.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Option contains some value using 
    /// <see cref="Option&lt;T&gt;.IsSome"/> or <see cref="Option&lt;T&gt;.IsNone"/>.
    /// </summary>
    /// <returns>The inner value of the Option.</returns>
    public T? Unwrap() =>
        Contents;

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action<T> whenSome, Action whenNone)
    {
        if (IsSome)
        {
            whenSome(Contents!);
        }
        else
        {
            whenNone();
        }

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action whenSome, Action whenNone) =>
        Tap(_ => whenSome(), whenNone);


    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(Action<T> whenSome) =>
        Tap(whenSome, () => { });

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(Action whenSome) =>
        TapSome(_ => whenSome());

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is None.
    /// </summary>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapNone(Action whenNone) =>
        Tap(_ => { }, whenNone);
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
    public static Option<T> Some<T>(this T entity) =>
        new(new Some<T>(entity));

    /// <summary>
    /// Create an Option that represents no value.
    /// </summary>
    /// <typeparam name="T">The type of the contents if they had been present.</typeparam>
    /// <returns>A new Option that represents a lack of contents.</returns>
    public static Option<T> None<T>() =>
        new(new None<T>());
}

/// <summary>
/// Represents no value in an Option type.
/// </summary>
/// <typeparam name="T">The inner type of the Option if it were Some.</typeparam>
[ExcludeFromCodeCoverage]
public record None<T>();

/// <summary>
/// Represents some value in an Option type.
/// </summary>
/// <typeparam name="T">The inner type of the Option.</typeparam>
/// <param name="Contents">The actual contents wrapped in the Option type.</param>
[ExcludeFromCodeCoverage]
public record Some<T>(T Contents);