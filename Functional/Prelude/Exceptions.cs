namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Obtains the inner exception message when present.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Exception withInnerException = new Exception("message", new Exception("inner exception"));
    /// Exception noInnerException = new Exception("message", null);
    /// 
    /// withInnerException
    ///     .InnerExceptionMessage()
    ///     // Prints the message when it's some.
    ///     .EffectSome(innerMessage => Console.WriteLine(innerMessage));
    ///     
    /// noInnerException
    ///     .InnerExceptionMessage()
    ///     // Doesn't print since there isn't an inner exception
    ///     .EffectSome(innerMessage => Console.WriteLine(innerMessage));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="exception">The exception to get the inner Exception message from.</param>
    /// <returns>An optional inner exception message.</returns>
    public static Option<string> InnerExceptionMessage(this Exception exception) =>
        exception
            .InnerException
            .Optional()
            .Map(exn => exn.Message);

    // TODO: Examples
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
            return toTry().Pipe(Ok);
        }
        catch (Exception ex)
        {
            return Error<TResult>(ex);
        }
    }

    // TODO: Examples
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
            return toTry(input).Pipe(Ok);
        }
        catch (Exception ex)
        {
            return Error<TResult>(ex);
        }
    }

    // TODO: Examples
    /// <summary>
    /// Try something async that might throw.
    /// </summary>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an Exception.</returns>
    public static async Task<Result<TResult, Exception>> TryAsync<TResult>(Func<Task<TResult>> toTry)
    {
        try
        {
            return await toTry().PipeAsync(Ok);
        }
        catch (Exception ex)
        {
            return Error<TResult>(ex);
        }
    }

    // TODO: Examples
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
                .PipeAsync(Ok);
        }
        catch (Exception ex)
        {
            return Error<TResult>(ex);
        }
    }

}