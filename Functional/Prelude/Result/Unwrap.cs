namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    public static async Task<bool> IsOk<Ok, Error>(this Task<Result<Ok, Error>> result) =>
        (await result).IsOk;

    // TODO: Examples
    public static async Task<bool> IsError<Ok, Error>(this Task<Result<Ok, Error>> result) =>
        (await result).IsError;

    // TODO: Examples
    /// <summary>
    /// Unwrap is used to get the inner value of a Result when the Result type
    /// is ok. If the result is an error, it will throw an InvalidOperationException.
    /// <br /><br />
    /// For example Result.Error&lt;string, Exception&gt;(new Exception("error")).Unwrap() will throw since
    /// there was no ok value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is ok using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was an error and was unwrapped as ok.</exception>
    public static async Task<Ok> UnwrapAsync<Ok, Error>(this Task<Result<Ok, Error>> result)
    {
        var theResult = await result;
        if (theResult.IsError) throw new InvalidOperationException("Failed to unwrap a result as an ok type because it was an Error. When using this method, be sure to check if the value is Ok by using the IsOk method beforehand.");
        return theResult.Unwrap();
    }

    // TODO: Examples
    /// <summary>
    /// UnwrapError is used to get the inner value of a Result when the Result type
    /// is an error. If the result is ok, it will throw an InvalidOperationException.
    /// <br /><br />
    /// For example Result.Ok&lt;string, int&gt;("hello").UnwrapError() will throw since
    /// there was no error value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is an error using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was an ok value and unwrapped as an error.</exception>
    public static async Task<Error> UnwrapErrorAsync<Ok, Error>(this Task<Result<Ok, Error>> result)
    {
        var theResult = await result;
        if (theResult.IsOk) throw new InvalidOperationException("Failed to unwrap the result as an error type because it was Ok. When using this method, be sure to check if the value is Error by using the IsError method beforehand. ");
        return theResult.UnwrapError();
    }
}