namespace Functional;

public static partial class Prelude
{
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
        if (result.IsError) return result.UnwrapError().Error<Output, Error>();

        var contents = result.Unwrap();

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

        if (awaited.IsError) return awaited.UnwrapError().Error<Output, Error>();

        var contents = awaited.Unwrap();

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

        if (awaited.IsError) return awaited.UnwrapError().Error<Output, Error>();

        var contents = awaited.Unwrap();

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
}
