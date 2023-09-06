using Functional.Monadic;
using Functional.Options;

namespace Functional.Exceptions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Obtains the inner exception message when present.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    public static Option<string> InnerExceptionMessage(this Exception exception) =>
        exception
            .InnerException
            .Optional()
            .Map(exn => exn.Message);

    /// <summary>
    /// Try an operation that may throw an exception.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <typeparam name="TResult">The resulting type.</typeparam>
    /// <param name="input">The input to operate on.</param>
    /// <param name="toTry">The function which may throw an exception.</param>
    /// <returns>A result which needs to be caught by the Catch method.</returns>
    public static TryResult<TResult> Try<T, TResult>(
        this T input,
        Func<T, TResult> toTry)
    {
        try
        {
            return toTry(input)
                .Pipe(res => TryResult.Success(res));
        }
        catch (Exception ex)
        {
            return TryResult.Failure<TResult>(ex);
        }
    }

    /// <summary>
    /// Catch a TryResult to handle successes and failures.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="tryResult">The result to be handled.</param>
    /// <param name="onSuccess">What to do when success occurs.</param>
    /// <param name="onFailure">What to do with the thrown exception.</param>
    /// <returns>The output result.</returns>
    public static TResult Catch<T, TResult>(
        this TryResult<T> tryResult,
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
            tryResult
                .Match(
                    success => onSuccess(success),
                    failure => onFailure(failure));

    /// <summary>
    /// Try an operation that may throw an exception async.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="input">The input to try the operation on.</param>
    /// <param name="toTry">The risky operation to try.</param>
    /// <returns>A TryResult to be handled with CatchAsync.</returns>
    public static async Task<TryResult<TResult>> TryAsync<T, TResult>(
        this T input,
        Func<T, Task<TResult>> toTry)
    {
        try
        {
            return await toTry(input)
                .PipeAsync(res => TryResult.Success(res));
        }
        catch (Exception ex)
        {
            return TryResult.Failure<TResult>(ex);
        }
    }

    /// <summary>
    /// Catch a TryResult and handle it.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">THe result type.</typeparam>
    /// <param name="tryResult">The result of trying the operation.</param>
    /// <param name="onSuccess">What to do when the operation succeeds.</param>
    /// <param name="onFailure">What to do with the exception that was thrown.</param>
    /// <returns>The output result.</returns>
    public static async Task<TResult> CatchAsync<T, TResult>(
        this Task<TryResult<T>> tryResult,
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
            (await tryResult)
                .Match(
                    success => onSuccess(success),
                    failure => onFailure(failure));

    /// <summary>
    /// Match the TryResult to either Success or Failure and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="tryResult">The result of trying something to be matched.</param>
    /// <param name="onSuccess">The fucntion to execute when TrySuccess.</param>
    /// <param name="onFailure">The function to execute when TryFailure.</param>
    /// <returns>The result of the function performed.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
    /// TryResult type other than TrySuccess or TryFailure.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<TryResult<TInput>> tryResult,
        Func<TInput, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
            (await tryResult)
                .Match(onSuccess, onFailure);

}

