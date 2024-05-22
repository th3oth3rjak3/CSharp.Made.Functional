namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="whenOk">An action to perform when ok.</param>
    /// <param name="whenError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action<Error> whenError) =>
        (await result)
            .Tap(whenOk, whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="whenOk">The action to perform when ok.</param>
    /// <param name="whenError">The action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action whenError) =>
        (await result)
            .Tap(whenOk, whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Func<Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) theResult.TapOk(whenOk);
        if (theResult.IsError) await whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Func<Error, Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) theResult.TapOk(whenOk);
        if (theResult.IsError) await whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action<Error> whenError) =>
        (await result)
            .Tap(whenOk, whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action whenError) =>
        (await result)
            .Tap(whenOk, whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Func<Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) whenOk();
        if (theResult.IsError) await whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Func<Error, Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) whenOk();
        if (theResult.IsError) await whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenOk, Action<Error> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk();
        if (theResult.IsError) whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenOk, Action whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk();
        if (theResult.IsError) whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenOk, Func<Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk();
        if (theResult.IsError) await whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Task> whenOk, Func<Error, Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk();
        if (theResult.IsError) await whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> whenOk, Action<Error> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk(theResult.Unwrap());
        if (theResult.IsError) whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> whenOk, Action whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk(theResult.Unwrap());
        if (theResult.IsError) whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> whenOk, Func<Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk(theResult.Unwrap());
        if (theResult.IsError) await whenError();

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into a result to perform a side-effect without consuming the result.
    /// </summary>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type when the result is error.</typeparam>
    /// <param name="result">The result to tap into.</param>
    /// <param name="whenOk">The action to perform when the result is ok.</param>
    /// <param name="whenError">The action to perform when the result is an error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Func<Ok, Task> whenOk, Func<Error, Task> whenError)
    {
        var theResult = await result;
        if (theResult.IsOk) await whenOk(theResult.Unwrap());
        if (theResult.IsError) await whenError(theResult.UnwrapError());

        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action<Ok>[] whenOk) =>
        (await result).TapOk(whenOk);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action[] whenOk) =>
        (await result).TapOk(whenOk);

    // TODO: Examples
    // TODO: remove async lambda
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Task>[] whenOk)
    {
        var theResult = await result;
        if (theResult.IsOk)
        {
            whenOk.ToList().ForEach(async action => await action());
        }
        return theResult;
    }

    // TODO: Examples
    // TODO: remove async lambda
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Ok, Task>[] whenOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        var contents = theResult.Unwrap();
        whenOk.ToList().ForEach(async action => await action(contents));
        return theResult;
    }

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action<Error>[] whenError) =>
        (await result)
            .TapError(whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Action[] whenError) =>
        (await result).TapError(whenError);

    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Task>[] whenError)
    {
        var theResult = await result;
        if (theResult.IsError)
        {
            whenError.ToList().ForEach(async action => await action());
        }
        return theResult;
    }

    // TODO: Remove async lambda
    // TODO: Examples
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, params Func<Error, Task>[] whenError)
    {
        var theResult = await result;
        if (theResult.IsError)
        {
            var contents = theResult.UnwrapError();
            whenError.ToList().ForEach(async action => await action(contents));
        }
        return theResult;
    }
}