using static Functional.Monadic.MonadicExtensions;

namespace Functional.Results;

public static class ResultExtensions
{
    /// <summary>
    /// When the result is a success, return its contents, otherwise return an alternate value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static TInput Reduce<TInput>(this Result<TInput> inputResult, TInput alternate) =>
        inputResult
            .Match(
                success => success.Contents,
                failure => alternate);

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When success, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static TInput Reduce<TInput>(this Result<TInput> inputResult, Func<TInput> alternate) =>
        inputResult
            .Match(
                success => success.Contents,
                failure => alternate());

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <param name="inputResult">The result to unpack.</param>
    /// <param name="alternate">A function which uses a failure 
    /// result to return an alternate.</param>
    /// <returns>When success, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static TInput Reduce<TInput>(this Result<TInput> inputResult, Func<FailureResult, TInput> alternate) =>
        inputResult
            .Match(
                success => success.Contents,
                failure => alternate(failure));

    /// <summary>
    /// When the result is a success, return its contents, otherwise return an alternate value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static async Task<TInput> ReduceAsync<TInput>(this Task<Result<TInput>> input, TInput alternate) =>
        await (await input)
            .Match(
                success => success.Contents,
                failure => alternate)
            .AsAsync();

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function which uses a failure 
    /// result to return an alternate.</param>
    /// <returns>When success, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static async Task<TInput> ReduceAsync<TInput>(this Task<Result<TInput>> input, Func<FailureResult, TInput> alternate) =>
        await (await input)
            .Match(
                success => success.Contents,
                failure => alternate(failure))
            .AsAsync();

    /// <summary>
    /// When the result is a success, return its contents, 
    /// otherwise execute the function to produce an alternate value.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <param name="input">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When success, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static async Task<TInput> ReduceAsync<TInput>(this Task<Result<TInput>> input, Func<TInput> alternate) =>
        await (await input)
            .Match(
                success => success.Contents,
                failure => alternate())
            .AsAsync();

    /// <summary>
    /// A function used to return a failure result as a Result of the provided type.
    /// </summary>
    /// <typeparam name="TResult">The type of result to return.</typeparam>
    /// <param name="failureResult">The failure result to convert into a Result.</param>
    /// <returns>The new Result.</returns>
    private static Result<TResult> MatchFailure<TResult>(FailureResult failureResult) =>
        failureResult
            .FMap(res => Result.Failure<TResult>(res.FailureMessages));

    /// <summary>
    /// A function used to return a FailureResult as a Result of the provided type.
    /// Returns as a Task for async Lambda expressions.
    /// </summary>
    /// <typeparam name="TResult">The resulting type.</typeparam>
    /// <param name="failureResult">The failure result to convert.</param>
    /// <returns>A Task with the new result.</returns>
    private static Task<Result<TResult>> MatchFailureAsync<TResult>(FailureResult failureResult) =>
        failureResult
            .FMap(result => (Result<TResult>)result)
            .AsAsync();

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="onSuccess">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static Result<TResult> Bind<TInput, TResult>(this Result<TInput> result, Func<TInput, Result<TResult>> onSuccess) =>
        result
            .Match(
                success =>
                    success
                        .Contents
                        .FMap(onSuccess),
                MatchFailure<TResult>);

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="onSuccess">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<TResult>> Bind<TInput, TResult>(this Result<TInput> result, Func<TInput, Task<Result<TResult>>> onSuccess) =>
        await result
            .Match(
                async success =>
                    await success
                        .Contents
                        .FMap(onSuccess),
                MatchFailureAsync<TResult>);

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="onSuccess">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<TResult>> BindAsync<TInput, TResult>(this Task<Result<TInput>> result, Func<TInput, Result<TResult>> onSuccess) =>
        (await result)
            .Match(
                success =>
                    success
                        .Contents
                        .FMap(onSuccess),
                MatchFailure<TResult>);

    /// <summary>
    /// Perform work on a previous result. When the result is successful, 
    /// perform work on the result by providing an onSuccess function.
    /// On failure, the previous failure will be returned as the new result type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result after performing 
    /// the onSuccess function.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="onSuccess">The function to perform when the 
    /// previous result is a SuccessResult.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<TResult>> BindAsync<TInput, TResult>(this Task<Result<TInput>> result, Func<TInput, Task<Result<TResult>>> onSuccess) =>
        await (await result)
            .Match(
                success =>
                    success
                        .Contents
                        .FMap(onSuccess),
                MatchFailureAsync<TResult>);

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TResult">The output contents of the new result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static Result<TResult> Map<TInput, TResult>(this Result<TInput> result, Func<TInput, TResult> mapper) =>
        result
            .Bind(success =>
                mapper(success).Success());

    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TResult">The output contents of the new result.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TResult>> MapAsync<TInput, TResult>(this Task<Result<TInput>> result, Func<TInput, TResult> mapper) =>
        (await result)
            .Bind(success =>
                mapper(success).Success());

}