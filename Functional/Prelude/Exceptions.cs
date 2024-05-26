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
    
    /// <summary>
    /// Try an action which may throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Try(() => Console.WriteLine("a message"))
    ///     .Match(
    ///         unit => "return type is unit",
    ///         exn => "return type is exception");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="toTry">The action to try.</param>
    /// <returns>The result of trying the action.</returns>
    public static Result<Unit, Exception> Try(Action toTry)
    {
        try
        {
            return Effect(toTry);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    /// <summary>
    /// Try something which could throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// "input"
    ///     .Try(Console.WriteLine)
    ///     .Match(
    ///         unit => "it printed successfully",
    ///         exn => "failure occurred while printing");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="input">The input to perform the action on.</param>
    /// <param name="toTry">The action to try.</param>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <returns>A result of trying the action.</returns>
    public static Result<Unit, Exception> Try<T>(this T input, Action<T> toTry)
    {
        try
        {
            return Effect(() => toTry(input));
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    /// <summary>
    /// Try something which could throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Before:
    /// try
    /// {
    ///     doSomethingThatThrows();
    /// }
    /// catch (Exception ex)
    /// {
    ///     // Deal with the exception.
    /// }
    ///
    /// // After:
    /// Try(() => doSomethingThatThrows())
    ///     .Match(
    ///         ok => "it was ok",
    ///         exn => exn.Message);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an exception.</returns>
    public static Result<TResult, Exception> Try<TResult>(Func<TResult> toTry)
    {
        try
        {
            return toTry();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
    
    /// <summary>
    /// Try something which could throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string GetSomeValue() => "some value";
    ///
    /// GetSomeValue()
    ///     .Try(input => int.Parse(input))
    ///     .Match(
    ///         ok => "it was an integer",
    ///         exn => exn.Message);
    /// </code>
    /// </example>
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
            return toTry(input);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    // TODO: Documentation
    // TODO: Examples
    public static async Task<Result<Unit, Exception>> TryAsync(Action toTry)
    {
        try
        {
            return await EffectAsync(toTry);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Documentation
    // TODO: Examples
    public static async Task<Result<Unit, Exception>> TryAsync<T>(this Task<T> input, Action<T> toTry)
    {
        try
        {
            return await input.EffectAsync(toTry);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    /// <summary>
    /// Try something async that might throw.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; TryGetIntegerString() => throw new Exception("not implemented yet");
    /// 
    /// await TryAsync(TryGetIntegerString)
    ///     .Match(
    ///         ok => 
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type when trying is Ok.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an Exception.</returns>
    public static async Task<Result<TResult, Exception>> TryAsync<TResult>(Func<Task<TResult>> toTry)
    {
        try
        {
            return await toTry();
        }
        catch (Exception ex)
        {
            return ex;
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
            return await toTry(await input);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Examples
    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static Result<Option<TMapped>, Exception> TryMap<T, TMapped>(
        this Option<T> optional,
        Func<T, TMapped> mapping)
        where TMapped : notnull
    {
        try
        {
            return optional.Map(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Examples
    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static Result<Option<TMapped>, Exception> TryMap<T, TMapped>(
        this Option<T> optional,
        Func<TMapped> mapping)
        where TMapped : notnull
    {
        try
        {
            return optional.Map(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Examples
    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, TMapped> mapping)
        where TMapped : notnull
        where T : notnull
    {
        try
        {
            return await optional.MapAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<TMapped> mapping)
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Task<TMapped>> mapping)
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Task<TMapped>> mapping)
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    public static Result<Option<TMapped>, Exception> TryBind<T, TMapped>(
        this Option<T> optional,
        Func<T, Option<TMapped>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        try
        {
            return optional.Bind(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    public static Result<Option<TMapped>, Exception> TryBind<T, TMapped>(
        this Option<T> optional,
        Func<Option<TMapped>> mapping)
        where TMapped : notnull
        where T : notnull
    {
        try
        {
            return optional.Bind(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Option<TMapped>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Option<TMapped>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Task<Option<TMapped>>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Task<Option<TMapped>>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    public static Result<TMapped, Exception> TryMap<T, TMapped>(
        this Result<T, Exception> result,
        Func<T, TMapped> mapping)
    {
        try
        {
            return result.Map(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    public static Result<TMapped, Exception> TryMap<T, TMapped>(
        this Result<T, Exception> result,
        Func<TMapped> mapping)
    {
        try
        {
            return result.Map(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, TMapped> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<TMapped> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Task<TMapped>> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Task<TMapped>> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Examples.
    /// <summary>
    /// Try to perform a binding function which could throw.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="toTry">A binding function which could throw.</param>
    /// <typeparam name="T">The original result Ok type.</typeparam>
    /// <typeparam name="TBoundResult">The new result Ok type.</typeparam>
    /// <returns>A result which either a mapped value or an exception.</returns>
    public static Result<TBoundResult, Exception> TryBind<T, TBoundResult>(
        this Result<T, Exception> result,
        Func<T, Result<TBoundResult, Exception>> toTry)
    {
        try
        {
            return result.Bind(toTry);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Examples.
    /// <summary>
    /// Try to perform a binding function which could throw.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="toTry">A binding function which could throw.</param>
    /// <typeparam name="T">The original result Ok type.</typeparam>
    /// <typeparam name="TBoundResult">The new result Ok type.</typeparam>
    /// <returns>A result which either a mapped value or an exception.</returns>
    public static Result<TBoundResult, Exception> TryBind<T, TBoundResult>(
        this Result<T, Exception> result,
        Func<Result<TBoundResult, Exception>> toTry)
    {
        try
        {
            return result.Bind(toTry);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Result<TMapped, Exception>> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Result<TMapped, Exception>> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Task<Result<TMapped, Exception>>> mapping)
    {
        throw new NotImplementedException();
    }
    
    // TODO: Unit tests.
    // TODO: Documentation
    // TODO: Examples
    // TODO: Implementation
    public static Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Task<Result<TMapped, Exception>>> mapping)
    {
        throw new NotImplementedException();
    }
}