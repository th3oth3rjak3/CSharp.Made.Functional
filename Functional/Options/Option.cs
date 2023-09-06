using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Options;

[ExcludeFromCodeCoverage]
public sealed record Option<T>
{
    private Union<Some<T>, None<T>> Contents { get; init; }

    public Option(Some<T> some) => Contents = new Union<Some<T>, None<T>>(some);

    public Option(None<T> none) => Contents = new Union<Some<T>, None<T>>(none);

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The fucntion to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSome, Func<TResult> whenNone) =>
        Contents
            .Match(
                some => whenSome(some.Contents),
                _ => whenNone());
}

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

[ExcludeFromCodeCoverage]
public record None<T>();

[ExcludeFromCodeCoverage]
public record Some<T>(T Contents);