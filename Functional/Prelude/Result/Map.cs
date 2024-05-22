namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    // TODO: Move to result class.
    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static Result<Output, Error> Map<Ok, Output, Error>(
        this Result<Ok, Error> result,
        Func<Ok, Output> mapper) =>
            result
                .Match(
                    ok => Result.Ok<Output, Error>(mapper(ok)),
                    Result.Error<Output, Error>);

    // TODO: Examples
    // TODO: Move to result class
    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static Result<Ok, NewError> MapError<Ok, Error, NewError>(
        this Result<Ok, Error> result,
        Func<Error, NewError> errorMapper) =>
        result
            .Match(
                Result.Ok<Ok, NewError>,
                error =>
                    errorMapper(error)
                        .Pipe(Result.Error<Ok, NewError>));

    // TODO: Examples
    /// <summary>
    /// Map a successful result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Output, Error>> MapAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> mapper)
    {
        var outcome = await result;

        return outcome.Map(mapper);
    }

    // TODO: Examples
    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="Output">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Output, Error>> MapAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> mapper)
    {
        var outcome = await result;

        if (outcome.IsOk)
        {
            var mapped = await mapper(outcome.Unwrap());
            return mapped.Ok<Output, Error>();
        }

        var err = outcome.UnwrapError();
        return err.Error<Output, Error>();
    }

    // TODO: Examples
    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static async Task<Result<Ok, NewError>> MapErrorAsync<Ok, Error, NewError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, NewError> errorMapper)
    {
        var outcome = await result;
        return outcome.MapError(errorMapper);
    }


    // TODO: Examples
    /// <summary>
    /// Map a result with one error type to another.
    /// </summary>
    /// <param name="result">The result to map the error of.</param>
    /// <param name="errorMapper">A function to transform one error to another.</param>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <typeparam name="Error">The type of the old error.</typeparam>
    /// <typeparam name="NewError">The type for the new error.</typeparam>
    /// <returns>A result with a mapped error.</returns>
    public static async Task<Result<Ok, NewError>> MapErrorAsync<Ok, Error, NewError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, Task<NewError>> errorMapper)
    {
        var outcome = await result;

        if (outcome.IsOk) return outcome.Unwrap().Ok<Ok, NewError>();

        var err = outcome.UnwrapError();
        var mapped = await errorMapper(err);
        return mapped.Error<Ok, NewError>();
    }
}
