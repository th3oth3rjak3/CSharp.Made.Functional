using System.Diagnostics.CodeAnalysis;

using Functional.Monadic;
using Functional.Options;

using OneOf;

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
    public static TryResult<TResult, Exception> Try<T, TResult>(
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
        this TryResult<T, Exception> tryResult,
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
            tryResult
                .Match(
                    success => onSuccess(success.Contents),
                    failure => onFailure(failure.Exception));

    /// <summary>
    /// Try an operation that may throw an exception async.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="input">The input to try the operation on.</param>
    /// <param name="toTry">The risky operation to try.</param>
    /// <returns>A TryResult to be handled with CatchAsync.</returns>
    public static async Task<TryResult<TResult, Exception>> TryAsync<T, TResult>(
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
        this Task<TryResult<T, Exception>> tryResult,
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
            (await tryResult)
                .Match(
                    success => onSuccess(success.Contents),
                    failure => onFailure(failure.Exception));

}

[GenerateOneOf]
[ExcludeFromCodeCoverage]
public partial class TryResult<T, TException>
    : OneOfBase<TrySuccess<T>, TryFailure<TException>> where TException : Exception
{
}

public static class TryResult
{
    public static TryResult<T, Exception> Success<T>(T input) =>
        new TrySuccess<T>(input);

    public static TryResult<T, Exception> Failure<T>(Exception exception) =>
        new TryFailure<Exception>(exception);
}

[ExcludeFromCodeCoverage]
public class TrySuccess<T>
{
    public TrySuccess(T contents) =>
        Contents = contents;
    public T Contents { get; init; }
}

[ExcludeFromCodeCoverage]
public class TryFailure<TException> where TException : Exception
{
    public TryFailure(TException exception) =>
        Exception = exception;
    public TException Exception { get; init; }
}