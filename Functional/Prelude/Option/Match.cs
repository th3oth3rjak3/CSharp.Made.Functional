namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull
    {
        var option = await optional;

        return option.IsNone
            ? whenNone()
            : whenSome(option.Unwrap());
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull
    {
        var result = await optional;

        if (result.IsNone) return whenNone();

        return await whenSome(result.Unwrap());
    }
    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var result = await optional;

        if (result.IsNone) return await whenNone();

        return whenSome(result.Unwrap());
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var result = await optional;

        if (result.IsNone) return await whenNone();

        return await whenSome(result.Unwrap());
    }

}
