namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> whenOk,
        Func<Error, Output> whenError)
    {
        var outcome = await result;

        return outcome
            .Match(whenOk, whenError);
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> whenOk,
        Func<Error, Task<Output>> whenError)
    {
        var outcome = await result;

        if (outcome.IsSuccess) return await whenOk(outcome.Unwrap());

        return await whenError(outcome.UnwrapFailure());
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> whenOk,
        Func<Error, Task<Output>> whenError)
    {
        var outcome = await result;

        if (outcome.IsSuccess) return whenOk(outcome.Unwrap());

        return await whenError(outcome.UnwrapFailure());
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> whenOk,
        Func<Error, Output> whenError)
    {
        var outcome = await result;

        if (outcome.IsSuccess) return await whenOk(outcome.Unwrap());

        return whenError(outcome.UnwrapFailure());
    }
}
