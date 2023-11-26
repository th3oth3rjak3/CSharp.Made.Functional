namespace Functional.Exceptions;

/// <summary>
/// Extensions to improve exception handling.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Obtains the inner exception message when present.
    /// </summary>
    /// <param name="exception">The exception to get the inner Exception message from.</param>
    /// <returns>An optional inner exception message.</returns>
    public static Option<string> InnerExceptionMessage(this Exception exception) =>
        exception
            .InnerException
            .Optional()
            .Map(exn => exn.Message);

    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an exception.</returns>
    public static Result<TResult, Exception> Try<TResult>(Func<TResult> toTry)
    {
        try
        {
            return toTry()
                .Pipe(Result.Ok<TResult, Exception>);
        }
        catch (Exception ex)
        {
            return Result.Error<TResult, Exception>(ex);
        }
    }

    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="input">The input to try something with.</param>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an exception.</returns>
    public static Result<TResult, Exception> Try<T, TResult>(this T input, Func<T, TResult> toTry)
    {
        try
        {
            return toTry(input)
                .Pipe(Result.Ok<TResult, Exception>);
        }
        catch (Exception ex)
        {
            return Result.Error<TResult, Exception>(ex);
        }
    }

    /// <summary>
    /// Try something async that might throw.
    /// </summary>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result is is Ok or an Exception.</returns>
    public static async Task<Result<TResult, Exception>> TryAsync<TResult>(Func<Task<TResult>> toTry)
    {
        try
        {
            return await toTry()
                .PipeAsync(Result.Ok<TResult, Exception>);
        }
        catch (Exception ex)
        {
            return await ex.AsAsync()
                .PipeAsync(Result.Error<TResult, Exception>);
        }
    }

    /// <summary>
    /// Try something async which could throw and Exception.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The output type when Ok.</typeparam>
    /// <param name="input">The input to try something on.</param>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an Exception.</returns>
    public static async Task<Result<TResult, Exception>> TryAsync<T, TResult>(this Task<T> input, Func<T, Task<TResult>> toTry)
    {
        try
        {
            return await input
                .PipeAsync(toTry)
                .PipeAsync(Result.Ok<TResult, Exception>);
        }
        catch (Exception ex)
        {
            return await ex
                .Pipe(Result.Error<TResult, Exception>)
                .AsAsync();
        }
    }
}

