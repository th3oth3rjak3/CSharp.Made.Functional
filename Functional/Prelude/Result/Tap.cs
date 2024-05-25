namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> onOk, Action<Error> onError) =>
        (await result)
            .Tap(onOk, onError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">The action to perform when ok.</param>
    /// <param name="onError">The action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> onOk, Action onError) =>
        (await result)
            .Tap(onOk, onError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> onOk, Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> onOk, Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action onOk, Action<Error> onError) =>
        (await result)
            .Tap(onOk, onError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action onOk, Action onError) =>
        (await result)
            .Tap(onOk, onError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action onOk, Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action onOk, Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> onOk, Action<Error> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> onOk, Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> onOk, Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> onOk, Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> onOk, Action<Error> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> onOk, Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> onOk, Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="onOk">The action to perform when the result is ok.</param>
    /// <param name="onError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> onOk, Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action<Ok>[] onOk) =>
        (await result).TapOk(onOk);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action[] onOk) =>
        (await result).TapOk(onOk);

    // TODO: Examples
    // TODO: remove async lambda
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsOk) await RunSequential(onOk);
        return theResult;
    }

    // TODO: Examples
    // TODO: remove async lambda
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsOk) await RunSequential(theResult.Unwrap(), onOk);
        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action<Error>[] onError) =>
        (await result)
            .TapError(onError);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action[] onError) =>
        (await result).TapError(onError);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsError) await RunSequential(onError);
        return theResult;
    }

    // TODO: Remove async lambda
    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsError) await RunSequential(theResult.UnwrapError(), onError);
        return theResult;
    }
}