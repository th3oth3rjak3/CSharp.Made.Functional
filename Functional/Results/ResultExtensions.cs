namespace Functional.Results;

/// <summary>
/// Extension methods to improve the functionality of the result type.
/// </summary>
public static class ResultExtensions
{
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

        if (outcome.IsOk) return await whenOk(outcome.Unwrap()!);

        return await whenError(outcome.UnwrapError()!);
    }

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

        if (outcome.IsOk) return whenOk(outcome.Unwrap()!);

        return await whenError(outcome.UnwrapError()!);
    }

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

        if (outcome.IsOk) return await whenOk(outcome.Unwrap()!);

        return whenError(outcome.UnwrapError()!);
    }

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static Result<Output, Error> Map<Ok, Output, Error>(
        this Result<Ok, Error> result,
        Func<Ok, Output> mapper) =>
            result
                .Match(
                    ok => Result.Ok<Output, Error>(mapper(ok)),
                    Result.Error<Output, Error>);

    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static Result<Ok, NewError> MapError<Ok, Error, NewError>(
        this Result<Ok, Error> result,
        Func<Error, NewError> errorMapper) =>
        result
            .Match(
                Result.Ok<Ok, NewError>,
                error =>
                    errorMapper(error)
                        .Pipe(Result.Error<Ok, NewError>));

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Output, Error>> MapAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> mapper)
    {
        var outcome = await result;

        return outcome.Map(mapper);
    }

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Output, Error>> MapAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> mapper)
    {
        var outcome = await result;

        if (outcome.IsOk)
        {
            var mapped = await mapper(outcome.Unwrap()!);
            return mapped.Ok<Output, Error>();
        }

        var err = outcome.UnwrapError()!;
        return err.Error<Output, Error>();
    }


    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static async Task<Result<Ok, NewError>> MapErrorAsync<Ok, Error, NewError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, NewError> errorMapper)
    {
        var outcome = await result;
        return outcome.MapError(errorMapper);
    }


    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static async Task<Result<Ok, NewError>> MapErrorAsync<Ok, Error, NewError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, Task<NewError>> errorMapper)
    {
        var outcome = await result;

        if (outcome.IsOk) return outcome.Unwrap()!.Ok<Ok, NewError>();

        var err = outcome.UnwrapError()!;
        var mapped = await errorMapper(err);
        return mapped.Error<Ok, NewError>();
    }

    /// <summary>
    /// When the result is Ok, return its contents, otherwise return an alternate value discarding the error.
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static Ok Reduce<Ok, Error>(
        this Result<Ok, Error> inputResult,
        Ok alternate) =>
        inputResult
            .Match(
                ok => ok,
                _ => alternate);

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value discarding the error.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When Ok, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static Ok Reduce<Ok, Error>(
        this Result<Ok, Error> inputResult,
        Func<Ok> alternate) =>
        inputResult
            .Match(
                ok => ok,
                _ => alternate());

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value using the error.
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function which uses an Error 
    /// result to return an alternate.</param>
    /// <returns>When Ok, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static Ok Reduce<Ok, Error>(
        this Result<Ok, Error> inputResult,
        Func<Error, Ok> alternate) =>
        inputResult
            .Match(
                ok => ok,
                alternate);

    /// <summary>
    /// When the result is Ok, return its contents, otherwise return an alternate value discarding the error.
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> input,
        Ok alternate) =>
            await (await input)
                .Match(
                    ok => ok,
                    _ => alternate)
                .AsAsync();

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function which uses an Error 
    /// result to return an alternate.</param>
    /// <returns>When Ok, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> input,
        Func<Error, Ok> alternate) =>
            await (await input)
                .Match(
                    ok => ok,
                    alternate)
                .AsAsync();

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value by discarding the error.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When Ok, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> input,
        Func<Ok> alternate) =>
            await (await input)
                .Match(
                    ok => ok,
                    _ => alternate())
                .AsAsync();

    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Output">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <typeparam name="Error">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public static Result<Output, Error> Bind<Ok, Output, Error>(
        this Result<Ok, Error> result,
        Func<Ok, Result<Output, Error>> binder)
    {
        if (result.IsError) return result.UnwrapError()!.Error<Output, Error>();

        var contents = result.Unwrap()!;

        return binder(contents);
    }

    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a binding function.
    /// On Error, the previous result will be returned as the new result type.
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Output">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <typeparam name="Error">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<Output, Error>> BindAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Result<Output, Error>> binder)
    {
        var awaited = await result;

        if (awaited.IsError) return awaited.UnwrapError()!.Error<Output, Error>();

        var contents = awaited.Unwrap()!;

        return binder(contents);
    }

    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a binding function.
    /// On Error, the previous result will be returned as the new result type.
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Output">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <typeparam name="Error">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<Output, Error>> BindAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Result<Output, Error>>> binder)
    {
        var awaited = await result;

        if (awaited.IsError) return awaited.UnwrapError()!.Error<Output, Error>();

        var contents = awaited.Unwrap()!;

        return await binder(contents);
    }

    /// <summary>
    /// Bind a List of Results to a Result of List of the inner object.
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="inputs"></param>
    /// <returns>A success result when all inner results are a success. A failure result when one or more failures occurred.</returns>
    public static Result<List<Ok>, List<Error>> BindAll<Ok, Error>(this List<Result<Ok, Error>> inputs) =>
        new
        {
            OutputSuccesses = new List<Ok>(),
            OutputFailures = new List<Error>()
        }
            .Pipe(mutableData =>
                inputs
                    .Select(input =>
                        input
                            .Match(
                                ok =>
                                {
                                    mutableData.OutputSuccesses.Add(ok);
                                    return true;
                                },
                                error =>
                                {
                                    mutableData.OutputFailures.Add(error);
                                    return false;
                                }))
                    .ToList()
                    .All(result => result == true)
                    .Pipe(wasSuccessful =>
                        wasSuccessful switch
                        {
                            true => mutableData.OutputSuccesses.Ok<List<Ok>, List<Error>>(),
                            false => mutableData.OutputFailures.Error<List<Ok>, List<Error>>()
                        }));

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action<Error> whenError) =>
        (await result)
            .Effect(whenOk, whenError);

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action<Error> whenError) =>
        (await result)
            .Effect(_ => whenOk(), whenError);

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk, Action whenError) =>
        (await result)
            .Effect(whenOk, _ => whenError());

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk, Action whenError) =>
        (await result)
            .Effect(_ => whenOk(), _ => whenError());

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Ok> whenOk) =>
        (await result)
            .Effect(whenOk, _ => { });

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenOk">Perform this action when the value is Ok.</param>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenOk) =>
        (await result)
            .Effect(_ => whenOk(), _ => { });

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action<Error> whenError) =>
        (await result)
            .Effect(_ => { }, whenError);


    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="result">The result to perform the side-effect on.</param>
    /// <param name="whenError">Perform this action when the value is Error.</param>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result, Action whenError) =>
        (await result)
            .Effect(_ => { }, _ => whenError());
}