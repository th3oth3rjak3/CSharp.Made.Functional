namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static Option<TResult> Map<T, TResult>(
        this Option<T> option,
        Func<T, TResult> mapper) =>
        option
            .Match(
                some => mapper(some).Optional(),
                Option.None<TResult>);

    /// <summary>
    /// When an Option is Some, map to a new value, ignoring the old value.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static Option<TResult> Map<T, TResult>(
        this Option<T> option,
        Func<TResult> mapper) =>
        option
            .Match(
                _ => mapper().Optional(),
                Option.None<TResult>);

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> option,
        Func<T, TResult> mapper)
    {
        var result = await option;
        return result.Map(mapper);
    }

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> option,
        Func<T, Task<TResult>> mapper)
    {
        var result = await option;

        if (result.IsNone) return Option.None<TResult>();

        return (await mapper(result.Unwrap()))
            .Optional();

    }

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Option<Task<T>> option,
        Func<T, TResult> mapper)
    {
        if (option.IsNone) return Option.None<TResult>();

        var contents = await option.Unwrap();

        return mapper(contents).Optional();
    }
}
