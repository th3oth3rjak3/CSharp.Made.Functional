namespace Functional.Results;

/// <summary>
/// Extension methods to improve the functionality of the result type.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Match the result to a success or failure and perform some function on either case.
    /// </summary>
    /// <typeparam name="TInput">The result input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <typeparam name="TError">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onSuccess">Perform some function on the success result.</param>
    /// <param name="onFailure">Perform some function on the failure result.</param>
    /// <returns>The result of executing the onSuccess or onFailure function.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other class 
    /// pretends to be a result.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, TResult> onSuccess,
        Func<TError, TResult> onFailure) =>
            (await result)
                .Match(onSuccess, onFailure);

    /// <summary>
    /// Match the result to a success or failure and perform some function on either case.
    /// </summary>
    /// <typeparam name="TInput">The result input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <typeparam name="TError">The error type of the initial result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onSuccess">Perform some function on the success result.</param>
    /// <param name="onFailure">Perform some function on the failure result.</param>
    /// <returns>The result of executing the onSuccess or onFailure function.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other class 
    /// pretends to be a result.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, Task<TResult>> onSuccess,
        Func<TError, Task<TResult>> onFailure) =>
            await (await result)
                .Match(
                    onSuccess,
                    onFailure);

    /// <summary>
    /// When the result is a success, return its contents, otherwise return an alternate value.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the input.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static TSuccess Reduce<TSuccess, TError>(
        this Result<TSuccess, TError> inputResult,
        TSuccess alternate) =>
        inputResult
            .Match(
                success => success,
                failure => alternate);

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="TSuccess">The input type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When success, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static TSuccess Reduce<TSuccess, TError>(
        this Result<TSuccess, TError> inputResult,
        Func<TSuccess> alternate) =>
        inputResult
            .Match(
                success => success,
                failure => alternate());

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// </summary>
    /// <typeparam name="TSuccess">The input type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function which uses a failure 
    /// result to return an alternate.</param>
    /// <returns>When success, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static TSuccess Reduce<TSuccess, TError>(
        this Result<TSuccess, TError> inputResult,
        Func<TError, TSuccess> alternate) =>
        inputResult
            .Match(
                success => success,
                failure => alternate(failure));

    /// <summary>
    /// When the result is a success, return its contents, otherwise return an alternate value.
    /// </summary>
    /// <typeparam name="TSuccess">The input type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static async Task<TSuccess> ReduceAsync<TSuccess, TError>(
        this Task<Result<TSuccess, TError>> input,
        TSuccess alternate) =>
            await (await input)
                .Match(
                    success => success,
                    failure => alternate)
                .AsAsync();

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// </summary>
    /// <typeparam name="TSuccess">The input type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function which uses a failure 
    /// result to return an alternate.</param>
    /// <returns>When success, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static async Task<TSuccess> ReduceAsync<TSuccess, TError>(
        this Task<Result<TSuccess, TError>> input,
        Func<TError, TSuccess> alternate) =>
            await (await input)
                .Match(
                    success => success,
                    failure => alternate(failure))
                .AsAsync();

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="TSuccess">The input type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When success, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static async Task<TSuccess> ReduceAsync<TSuccess, TError>(
        this Task<Result<TSuccess, TError>> input,
        Func<TSuccess> alternate) =>
            await (await input)
                .Match(
                    success => success,
                    failure => alternate())
                .AsAsync();

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TSuccess">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <typeparam name="TError">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static Result<TSuccess, TError> Bind<TInput, TSuccess, TError>(
        this Result<TInput, TError> result,
        Func<TInput, Result<TSuccess, TError>> binder) =>
            result
                .Map(success => binder(success))
                .Reduce(Result.Failure<TSuccess, TError>);

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TSuccess">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <typeparam name="TError">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<TSuccess, TError>> BindAsync<TInput, TSuccess, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, Result<TSuccess, TError>> binder) =>
            await result
                .MapAsync(binder)
                .ReduceAsync(Result.Failure<TSuccess, TError>);

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TSuccess">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <typeparam name="TError">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<TSuccess, TError>> BindAsync<TInput, TSuccess, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, Task<Result<TSuccess, TError>>> binder) =>
            await result
                .MapAsync(binder)
                .ReduceAsync(Result.Failure<TSuccess, TError>);

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TSuccess">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static Result<TSuccess, TError> Map<TInput, TSuccess, TError>(
        this Result<TInput, TError> result,
        Func<TInput, TSuccess> mapper) =>
            result
                .Match(
                    success => Result.Success<TSuccess, TError>(mapper(success)),
                    failure => Result.Failure<TSuccess, TError>(failure));

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TSuccess">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TSuccess, TError>> MapAsync<TInput, TSuccess, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, TSuccess> mapper) =>
            (await result)
                .Map(mapper);

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TSuccess">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TSuccess, TError>> MapAsync<TInput, TSuccess, TError>(
        this Task<Result<TInput, TError>> result,
        Func<TInput, Task<TSuccess>> mapper) =>
            await (await result)
                .Match(
                    success => mapper(success).PipeAsync(Result.Success<TSuccess, TError>),
                    failure => failure.Pipe(Result.Failure<TSuccess, TError>).AsAsync());

    /// <summary>
    /// Bind a List of Results to a Result of List of the inner object.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="inputs"></param>
    /// <returns>A success result when all inner results are a success. A failure result when one or more failures occurred.</returns>
    public static Result<List<TInput>, List<TError>> BindAll<TInput, TError>(this List<Result<TInput, TError>> inputs) =>
        new
        {
            OutputSuccesses = new List<TInput>(),
            OutputFailures = new List<TError>()
        }
            .Pipe(mutableData =>
                inputs
                    .Select(input =>
                        input
                            .Match(
                                success =>
                                {
                                    mutableData.OutputSuccesses.Add(success);
                                    return true;
                                },
                                failure =>
                                {
                                    mutableData.OutputFailures.Add(failure);
                                    return false;
                                }))
                    .ToList()
                    .All(result => result == true)
                    .Pipe(wasSuccessful =>
                        wasSuccessful switch
                        {
                            true => Result.Success<List<TInput>, List<TError>>(mutableData.OutputSuccesses),
                            false => Result.Failure<List<TInput>, List<TError>>(mutableData.OutputFailures)
                        }));
}