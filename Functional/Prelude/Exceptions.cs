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

    /// <summary>
    /// Try an action which may throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await TryAsync(() => Console.WriteLine("a message"))
    ///     .MatchAsync(
    ///         unit => "return type is unit",
    ///         exn => "return type is exception");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="toTry">The action to try.</param>
    /// <returns>The result of trying the action.</returns>
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

    /// <summary>
    /// Try an action which may throw an exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await "input"
    ///     .TryAsync(Console.WriteLine)
    ///     .MatchAsync(
    ///         unit => "return type is unit",
    ///         exn => "return type is exception");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="input">The input used in the action to try.</param>
    /// <param name="toTry">The action to try.</param>
    /// <returns>The result of trying the action.</returns>
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
    /// string result = 
    ///     await TryAsync(TryGetIntegerString)
    ///         .MatchAsync(
    ///             ok => ok,
    ///             error => error.Message);
    ///             
    /// Assert.AreEqual(result, "not implemented yet");
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

    /// <summary>
    /// Try something async which could throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; TryGetIntegerString(int input) => throw new Exception("not implemented yet");
    /// 
    /// string result = 
    ///     await 42
    ///         .Async()
    ///         .TryAsync(TryGetIntegerString)
    ///         .MatchAsync(
    ///             ok => ok,
    ///             error => error.Message);
    ///             
    /// Assert.AreEqual(result, "not implemented yet");
    /// </code>
    /// </example>
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

    /// <summary>
    /// Try something async that might throw.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task TryDoWork() => Task.Run(() => throw new Exception("not implemented yet"));
    /// 
    /// string result = 
    ///     await TryAsync(TryDoWork)
    ///         .MatchAsync(
    ///             ok => ok,
    ///             error => error.Message);
    ///             
    /// Assert.AreEqual(result, "not implemented yet");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an Exception.</returns>
    public static async Task<Result<Unit, Exception>> TryAsync(Func<Task> toTry)
    {
        try
        {
            await toTry();
            return Unit();
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Try something async which could throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task TryDoWork() => Task.Run(() => throw new Exception("not implemented yet"));
    /// 
    /// string result = 
    ///     await 42
    ///         .Async()
    ///         .TryAsync(TryDoWork)
    ///         .MatchAsync(
    ///             ok => ok,
    ///             error => error.Message);
    ///             
    /// Assert.AreEqual(result, "not implemented yet");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input to try something on.</param>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A result that is Ok or an Exception.</returns>
    public static async Task<Result<Unit, Exception>> TryAsync<T>(this Task<T> input, Func<Task> toTry)
    {
        try
        {
            await input;
            await toTry();
            return Unit();
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     Some("maybe an integer")
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     Some("42")
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     None&lt;string&gt;()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
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

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     Some("100")
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMap(() => int.Parse("won't parse"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     Some("anything")
    ///         .TryMap(() => int.Parse("42"));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     None&lt;string&gt;()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMap(() => int.Parse("input was None, so this won't matter."));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
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

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     await Some("maybe an integer")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Some("42")
    ///         .Async()
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
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

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     await Some("100")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(() => int.Parse("won't parse"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Some("anything")
    ///         .Async()
    ///         .TryMapAsync(() => int.Parse("42"));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMapAsync(() => int.Parse("input was None, so this won't matter."));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<TMapped> mapping)
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

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     await Some("maybe an integer")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Some("42")
    ///         .Async()
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Task<TMapped>> mapping)
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

    /// <summary>
    /// Perform a mapping operation on an Option when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;Option&lt;int&gt;, Exception&gt; mapped = 
    ///     await Some("100")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(() => int.Parse("won't parse").Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Some("anything")
    ///         .Async()
    ///         .TryMapAsync(() => int.Parse("42").Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), Some(42));
    /// 
    /// mapped =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         // Since the input is None, the mapping won't take place and the output will be Ok and None.
    ///         .TryMapAsync(() => int.Parse("input was None, so this won't matter.").Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TMapped">The output type of the option.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryMapAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Task<TMapped>> mapping)
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

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? None&lt;string&gt;()
    ///             : Some(input.ToString());
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     Some(8)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     Some(42)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     Some(15)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     None&lt;int&gt;()
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
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

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; Unsafe() => throw new Exception("error");
    /// Option&lt;string&gt; GetString() => Some("value");
    /// Option&lt;string&gt; NoneString() => None&lt;string&gt;();
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     Some(8)
    ///         .TryBind(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     Some(42)
    ///         .TryBind(NoneString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     Some(15)
    ///         .TryBind(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     None&lt;int&gt;()
    ///         .TryBind(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
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

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? None&lt;string&gt;()
    ///             : Some(input.ToString());
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     await Some(8)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     await Some(42)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Some(15)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Option<TMapped>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        try
        {
            return await optional.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; Unsafe() => throw new Exception("error");
    /// Option&lt;string&gt; GetString() => Some("value");
    /// Option&lt;string&gt; NoneString() => None&lt;string&gt;();
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     await Some(8)
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     await Some(42)
    ///         .Async()
    ///         .TryBindAsync(NoneString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Some(15)
    ///         .Async()
    ///         .TryBindAsync(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Option<TMapped>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        try
        {
            return await optional.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Option&lt;string&gt;&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? None&lt;string&gt;().Async()
    ///             : Some(input.ToString()).Async();
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     await Some(8)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     await Some(42)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Some(15)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<T, Task<Option<TMapped>>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        try
        {
            return await optional.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Option&lt;string&gt;&gt; Unsafe() => throw new Exception("error");
    /// Task&lt;Option&lt;string&gt;&gt; GetString() => Some("value").Async();
    /// Task&lt;Option&lt;string&gt;&gt; NoneString() => None&lt;string&gt;().Async();
    ///             
    /// Result&lt;Option&lt;string&gt;, Exception&gt; mapped = 
    ///     await Some(8)
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     await Some(42)
    ///         .Async()
    ///         .TryBindAsync(NoneString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Some(15)
    ///         .Async()
    ///         .TryBindAsync(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="optional">The option to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<Option<TMapped>, Exception>> TryBindAsync<T, TMapped>(
        this Task<Option<T>> optional,
        Func<Task<Option<TMapped>>> mapping)
        where T : notnull
        where TMapped : notnull
    {
        try
        {
            return await optional.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     Ok("not an integer")
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     Ok("42")
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 42);
    /// 
    /// mapped =
    ///     Error&lt;string&gt;("an existing error")
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMap(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
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

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     Ok("100")
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMap(() => int.Parse("something that won't parse"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     Ok("42")
    ///         .TryMap(() => int.Parse("100"));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 100);
    /// 
    /// mapped =
    ///     Error&lt;string&gt;("an existing error")
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMap(() => int.Parse("anything"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
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

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Ok("not an integer")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Ok("42")
    ///         .Async()
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 42);
    /// 
    /// mapped =
    ///     await Error&lt;string&gt;("an existing error")
    ///         .Async()
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMapAsync(value => int.Parse(value));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, TMapped> mapping)
    {
        try
        {
            return await result.MapAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Ok("100")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(() => int.Parse("something that won't parse"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Ok("42")
    ///         .Async()
    ///         .TryMapAsync(() => int.Parse("100"));
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 100);
    /// 
    /// mapped =
    ///     await Error&lt;string&gt;("an existing error")
    ///         .Async()
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMapAsync(() => int.Parse("anything"));
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<TMapped> mapping)
    {
        try
        {
            return await result.MapAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int&gt; mapped = 
    ///     await Ok("not an integer")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Ok("42")
    ///         .Async()
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 42);
    /// 
    /// mapped =
    ///     await Error&lt;string&gt;("an existing error")
    ///         .Async()
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMapAsync(value => int.Parse(value).Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Task<TMapped>> mapping)
    {
        try
        {
            return await result.MapAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Perform a mapping operation on Result when the mapping may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int&gt; mapped = 
    ///     await Ok("100")
    ///         .Async()
    ///         // This will throw an exception when it can't parse the input to an integer
    ///         // resulting in "mapped" being an Error Result.
    ///         .TryMapAsync(() => int.Parse("something that won't parse").Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// 
    /// mapped = 
    ///     await Ok("42")
    ///         .Async()
    ///         .TryMapAsync(() => int.Parse("100").Async());
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap(), 100);
    /// 
    /// mapped =
    ///     await Error&lt;string&gt;("an existing error")
    ///         .Async()
    ///         // Since the input is already an Error, the mapping won't take place and the output will be the same error.
    ///         .TryMapAsync(() => int.Parse("anything").Async());
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an existing error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform a mapping to.</param>
    /// <param name="mapping">The mapping function to perform.</param>
    /// <typeparam name="T">The type of the result Ok type.</typeparam>
    /// <typeparam name="TMapped">The output type of the result.</typeparam>
    /// <returns>The result of mapping.</returns>
    public static async Task<Result<TMapped, Exception>> TryMapAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Task<TMapped>> mapping)
    {
        try
        {
            return await result.MapAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? Error&lt;string&gt;("value too high")
    ///             : Ok(input.ToString());
    ///             
    /// Result&lt;string&gt; mapped = 
    ///     Ok(8)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     Ok(42)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.IsTrue(mapped.UnwrapError().Message, "value too high");
    /// 
    /// mapped = 
    ///     Ok(15)
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     Error&lt;int&gt;("there was an error")
    ///         .TryBind(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "there was an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TBoundResult">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="toTry">The binding function.</param>
    /// <returns>The result of binding.</returns>
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

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; Unsafe() => throw new Exception("error");
    /// Result&lt;string, Exception&gt; GetString() => Ok("value");
    /// Result&lt;string, Exception&gt; ErrorString() => Error&lt;string&gt;("Something bad happened");
    ///             
    /// Result&lt;string, Exception&gt; mapped = 
    ///     Ok(8)
    ///         .TryBind(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     Ok(42)
    ///         .TryBind(ErrorString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     Ok(15)
    ///         .TryBind(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     Error&lt;int&gt;("an error")
    ///         .TryBind(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TBoundResult">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input result.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="toTry">The binding function.</param>
    /// <returns>The result of binding.</returns>
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

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? Error&lt;string&gt;("value too high")
    ///             : Ok(input.ToString());
    ///             
    /// Result&lt;string&gt; mapped = 
    ///     await Ok(8)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.IsTrue(mapped.UnwrapError().Message, "value too high");
    /// 
    /// mapped = 
    ///     await Ok(15)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     await Error&lt;int&gt;("there was an error")
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "there was an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Result<TMapped, Exception>> mapping)
    {
        try
        {
            return await result.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; Unsafe() => throw new Exception("error");
    /// Result&lt;string, Exception&gt; GetString() => Ok("value");
    /// Result&lt;string, Exception&gt; ErrorString() => Error&lt;string&gt;("Something bad happened");
    ///             
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(8)
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .TryBindAsync(ErrorString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Ok(15)
    ///         .Async()
    ///         .TryBindAsync(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     await Error&lt;int&gt;("an error")
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input result.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Result<TMapped, Exception>> mapping)
    {
        try
        {
            return await result.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Result&lt;string, Exception&gt;&gt; UnsafeToString(int input) =>
    ///     input &lt; 10
    ///         ? throw new Exception("value too low")
    ///         : input &gt; 30
    ///             ? Error&lt;string&gt;("value too high").Async()
    ///             : Ok(input.ToString()).Async();
    ///             
    /// Result&lt;string&gt; mapped = 
    ///     await Ok(8)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "value too low");
    /// 
    /// mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.IsTrue(mapped.UnwrapError().Message, "value too high");
    /// 
    /// mapped = 
    ///     await Ok(15)
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "15");
    /// 
    /// mapped =
    ///     await Error&lt;int&gt;("there was an error")
    ///         .Async()
    ///         .TryBindAsync(UnsafeToString);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "there was an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input option.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<T, Task<Result<TMapped, Exception>>> mapping)
    {
        try
        {
            return await result.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces a Result type and may throw an Exception.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; Unsafe() => throw new Exception("error");
    /// Result&lt;string, Exception&gt; GetString() => Ok("value");
    /// Result&lt;string, Exception&gt; ErrorString() => Error&lt;string&gt;("Something bad happened");
    ///             
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(8)
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// 
    /// mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .TryBindAsync(ErrorString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.IsTrue(mapped.Unwrap().IsNone);
    /// 
    /// mapped = 
    ///     await Ok(15)
    ///         .Async()
    ///         .TryBindAsync(GetString);
    ///         
    /// Assert.IsTrue(mapped.IsOk);
    /// Assert.AreEqual(mapped.Unwrap().Unwrap(), "value");
    /// 
    /// mapped =
    ///     await Error&lt;int&gt;("an error")
    ///         .Async()
    ///         .TryBindAsync(Unsafe);
    ///         
    /// Assert.IsTrue(mapped.IsError);
    /// Assert.AreEqual(mapped.UnwrapError().Message, "an error");
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMapped">The type of the output.</typeparam>
    /// <typeparam name="T">The inner type of the input result.</typeparam>
    /// <param name="result">The result to try binding.</param>
    /// <param name="mapping">The binding function.</param>
    /// <returns>The result of binding.</returns>
    public static async Task<Result<TMapped, Exception>> TryBindAsync<T, TMapped>(
        this Task<Result<T, Exception>> result,
        Func<Task<Result<TMapped, Exception>>> mapping)
    {
        try
        {
            return await result.BindAsync(mapping);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
}