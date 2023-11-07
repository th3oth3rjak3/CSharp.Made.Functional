namespace Functional.Options;

/// <summary>
/// A wrapper class which contains either Some value or no value (None).
/// </summary>
/// <typeparam name="T">The inner type of the Option.</typeparam>
public sealed record Option<T>
{
    private Union<Some<T>, None<T>> Contents { get; init; }

    /// <summary>
    /// Construct a new Option from a Some.
    /// </summary>
    /// <param name="some">A Some object which contains value.</param>
    public Option(Some<T> some) => Contents = new Union<Some<T>, None<T>>(some);

    /// <summary>
    /// Construct a new Option from a None.
    /// </summary>
    /// <param name="none">A None object with no value.</param>
    public Option(None<T> none) => Contents = new Union<Some<T>, None<T>>(none);

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSome">The fucntion to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSome, Func<TResult> whenNone) =>
        Contents
            .Match(
                some => whenSome(some.Contents),
                _ => whenNone());

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public void Effect(Action<T> doWhenSome, Action doWhenNone) =>
        this.Contents
            .Effect(some => doWhenSome(some.Contents), _ => doWhenNone());

    /// <summary>
    /// Determine if the option contains some value. True when Some, otherwise false.
    /// </summary>
    public bool IsSome =>
        this.Contents.Match(_ => true, _ => false);

    /// <summary>
    /// Determine if the option contains no value. True when None, otherwise false.
    /// </summary>
    public bool IsNone =>
        this.Contents.Match(_ => false, _ => true);
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
    /// Create an Option that respresents no value.
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