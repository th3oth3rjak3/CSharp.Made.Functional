namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a binding function.
    /// On Error, the previous result will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// </code>
    /// </example>
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

        if (awaited.IsFailure) return Failure<Output, Error>(awaited.UnwrapFailure());

        var contents = awaited.Unwrap();

        return binder(contents);
    }

    // TODO: Examples
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

        if (awaited.IsFailure) return Failure<Output, Error>(awaited.UnwrapFailure());

        var contents = awaited.Unwrap();

        return await binder(contents);
    }
}