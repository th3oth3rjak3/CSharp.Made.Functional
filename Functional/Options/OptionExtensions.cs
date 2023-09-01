using Functional.Monadic;

namespace Functional.Options;
public static class OptionExtensions
{
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Optional<T>(this T? entity) =>
        entity switch
        {
            null => Option.None<T>(),
            _ => Option.Some(entity)
        };

    public static async Task<Option<T>> Optional<T>(this Task<T?> entity) =>
        (await entity) switch
        {
            T contents when contents is not null => Option.Some(contents),
            _ => Option.None<T>()
        };

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapper) =>
        option
            .Match(
                some => mapper(some.Contents).Optional(),
                none => Option.None<TResult>());

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, TResult> mapper) =>
        (await option)
            .Match(
                some => some.Contents.Pipe(mapper).Optional(),
                none => Option.None<TResult>());

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Task<Option<T>> option, Func<T, Task<TResult>> mapper) =>
        await (await option)
            .Match(
                async some => (await some.Contents.Pipe(mapper)).Optional(),
                none => Option.None<TResult>().AsAsync());

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(this Option<Task<T>> option, Func<T, TResult> mapper) =>
        await option
            .Match(
                async some => mapper(await some.Contents).Optional(),
                none => Option.None<TResult>().AsAsync());

    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) =>
        option
            .Match(
                some =>
                    predicate(some.Contents) switch
                    {
                        true => some,
                        false => Option.None<T>()
                    },
                none => Option.None<T>());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static T Reduce<T>(this Option<T> optional, Func<T> alternate) =>
        optional
            .Match(
                some => some.Contents,
                none => alternate());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static T Reduce<T>(this Option<T> optional, T alternate) =>
        optional
            .Match(
                some => some.Contents,
                none => alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, Func<T> alternate) =>
        (await optional)
            .Match(
                some => some.Contents,
                none => alternate());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, T alternate) =>
        (await optional)
            .Match(
                some => some.Contents,
                none => alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, Task<T> alternate) =>
        await (await optional)
            .Match(
                some => some.Contents.AsAsync(),
                async none => await alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, Func<Task<T>> alternate) =>
        await (await optional)
            .Match(
                some => some.Contents.AsAsync(),
                async none => await alternate());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Option<Task<T>> optional, T alternate) =>
        await optional
            .Match(
                async some => await some.Contents,
                none => alternate.AsAsync());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Option<Task<T>> optional, Func<T> alternate) =>
    await optional
        .Match(
            async some => await some.Contents,
            none => alternate().AsAsync());

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Option<Task<T>> optional, Task<T> alternate) =>
    await optional
        .Match(
            async some => await some.Contents,
            none => alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Option<Task<T>> optional, Func<Task<T>> alternate) =>
    await optional
        .Match(
            async some => await some.Contents,
            none => alternate());
}
