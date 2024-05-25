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
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Error, Output> onError)
    {
        var outcome = await result;

        return outcome
            .Match(onOk, onError);
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Error, Task<Output>> onError)
    {
        var outcome = await result;

        if (outcome.IsOk) return await onOk(outcome.Unwrap());

        return await onError(outcome.UnwrapError());
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Error, Task<Output>> onError)
    {
        var outcome = await result;

        if (outcome.IsOk) return onOk(outcome.Unwrap());

        return await onError(outcome.UnwrapError());
    }

    // TODO: Examples
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Error, Output> onError)
    {
        var outcome = await result;

        if (outcome.IsOk) return await onOk(outcome.Unwrap());

        return onError(outcome.UnwrapError());
    }
}