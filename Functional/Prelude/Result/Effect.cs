namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action<Error> whenError) =>
        (await result)
            .Effect(whenOk, whenError);

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action<Error> whenError) =>
        (await result)
            .Effect(_ => whenOk(), whenError);

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action whenError) =>
        (await result)
            .Effect(whenOk, _ => whenError());

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action whenError) =>
        (await result)
            .Effect(_ => whenOk(), _ => whenError());

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner value is ok.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk) =>
        (await result)
            .Effect(whenOk, _ => { });

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner value is ok, consuming the result.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk) =>
        (await result)
            .Effect(_ => whenOk(), _ => { });

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner value is ok, consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <param name="result">The result to act on.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenOk)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk();

        return Unit();
    }

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner value is an error, consuming the result.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Error> whenError) =>
        (await result)
            .Effect(_ => { }, whenError);

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner value is an error, consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when it's ok.</typeparam>
    /// <typeparam name="Error">The type of the result when it's an error.</typeparam>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenError) =>
        (await result)
            .Effect(_ => { }, _ => whenError());

    // TODO: Examples
    /// <summary>
    /// Perform a side-effect on a result type when the inner type is Error, consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to act on.</param>
    /// <param name="whenError">The action to perform when the inner value is an error.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsError) await whenError();
        return Unit();
    }
}

