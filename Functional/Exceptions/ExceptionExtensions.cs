namespace Functional.Exceptions;

/// <summary>
/// Extensions to improve exception handling.
/// </summary>
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

}

