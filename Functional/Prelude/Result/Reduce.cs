namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    // TODO: Move to result class
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

    // TODO: Examples
    // TODO: Move to result class.
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

    // TODO: Examples
    // TODO: move to result class.
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

    // TODO: Examples
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
                .Async();

    // TODO: Examples
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
                .Async();

    // TODO: Examples
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
                .Async();
}