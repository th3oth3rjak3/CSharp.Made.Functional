namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction(string input) => message = input;
    /// void noneAction() => message = "None";
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "hello, world!" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome, Action doWhenNone) where T : notnull =>
        (await optional)
            .Effect(doWhenSome, doWhenNone);

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction(string input) => message = input;
    /// Task noneAction() 
    /// {
    ///     message = "None";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "hello, world!" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome, Func<Task> doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) doWhenSome(theOption.Unwrap());
        if (theOption.IsNone) await doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction() => message = "Some";
    /// void noneAction() => message = "None";
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "Some" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action doWhenSome, Action doWhenNone) where T : notnull =>
        (await optional)
            .Effect(_ => doWhenSome(), doWhenNone);

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction() => message = "Some";
    /// Task noneAction() 
    /// { 
    ///     message = "None";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "Some" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action doWhenSome, Func<Task> doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) doWhenSome();
        if (theOption.IsNone) await doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction(string input) 
    /// {
    ///     message = input;
    ///     return Task.CompletedTask;   
    /// }
    /// void noneAction() => message = "None";
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "hello, world!" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Func<T, Task> doWhenSome, Action doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await doWhenSome(theOption.Unwrap());
        if (theOption.IsNone) doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction(string input) 
    /// { 
    ///     message = input;
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// Task noneAction()
    /// {
    ///     message = "None";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "hello, world!" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Func<T, Task> doWhenSome, Func<Task> doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await doWhenSome(theOption.Unwrap());
        if (theOption.IsNone) await doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction() 
    /// {
    ///     message = input;
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// void noneAction() => message = "None";
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "Some" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenSome, Action doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await doWhenSome();
        if (theOption.IsNone) doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction() 
    /// {
    ///     message = "Some";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// Task noneAction() 
    /// {
    ///     message = "None";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("hello, world!")
    ///     .Async()
    ///     // This will store "Some" in the message variable since the input is Some.
    ///     .EffectAsync(
    ///         someAction
    ///         noneAction);
    ///         
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This will store "None" in the message variable since the input is None.
    ///     .EffectAsync(
    ///         someAction,
    ///         noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenSome, Func<Task> doWhenNone) where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await doWhenSome();
        if (theOption.IsNone) await doWhenNone();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type when the inner value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction(string input) => message = input;
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This sets message to "123" since the input is Some.
    ///     .EffectSomeAsync(someAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This doesn't do anything since the input is None.
    ///     .EffectSomeAsync(someAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        params Action<T>[] doWhenSome) where T : notnull =>
        (await optional)
            .EffectSome(doWhenSome);

    // todo: examples
    // todo: tests
    // todo: docs
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        params Action<T>[] doWhenSome)
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return processingOrder == ProcessingOrder.Sequential
            ? await RunSequential(theOption.Unwrap(), doWhenSome)
            : await RunParallel(theOption.Unwrap(), doWhenSome);
    }

    // todo: examples
    // todo: tests
    // todo: docs
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        CancellationToken cancellationToken,
        params Action<T>[] doWhenSome)
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return await RunSequential(theOption.Unwrap(), doWhenSome, cancellationToken);
    }

    // todo: examples
    // todo: tests
    // todo: docs
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<T>[] doWhenSome)
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return (processingOrder == ProcessingOrder.Sequential)
            ? await RunSequential(theOption.Unwrap(), doWhenSome, cancellationToken)
            : await RunParallel(theOption.Unwrap(), doWhenSome, cancellationToken);
    }


    /// <summary>
    /// Perform a side effect on an option type when the inner value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void someAction() => message = "Some";
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This sets message to "Some" since the input is Some.
    ///     .EffectSomeAsync(someAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This doesn't do anything since the input is None.
    ///     .EffectSomeAsync(someAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        params Action[] doWhenSome)
        where T : notnull =>
            (await optional)
                .EffectSome(doWhenSome);

    // todo: docs
    // todo: examples
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        params Action[] doWhenSome)
    where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return (processingOrder == ProcessingOrder.Sequential)
            ? await RunSequential(doWhenSome)
            : await RunParallel(doWhenSome);
    }

    // todo: docs
    // todo: examples
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        CancellationToken cancellationToken,
        params Action[] doWhenSome)
    where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return await RunSequential(doWhenSome, cancellationToken);
    }

    // todo: docs
    // todo: examples
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] doWhenSome)
    where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return Unit();
        return (processingOrder == ProcessingOrder.Sequential)
            ? await RunSequential(doWhenSome, cancellationToken)
            : await RunParallel(doWhenSome, cancellationToken);
    }

    /// <summary>
    /// Perform a side effect on an option type when the inner value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction(string input)
    /// {
    ///     message = input;
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This sets message to "123" since the input is Some.
    ///     .EffectSomeAsync(someAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This doesn't do anything since the input is None.
    ///     .EffectSomeAsync(someAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        params Func<T, Task>[] doWhenSome)
        where T : notnull
    {
        var option = await optional;
        return option.IsSome
            ? await RunSequential(option.Unwrap(), doWhenSome)
            : Unit();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        params Func<T, Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        CancellationToken cancellationToken,
        params Func<T, Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<T, Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Perform a side effect on an option type when the inner value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task someAction()
    /// {
    ///     message = "Some";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This sets message to "Some" since the input is Some.
    ///     .EffectSomeAsync(someAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This doesn't do anything since the input is None.
    ///     .EffectSomeAsync(someAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        params Func<Task>[] doWhenSome)
        where T : notnull
    {
        var option = await optional;
        return option.IsSome
            ? await RunSequential(doWhenSome)
            : Unit();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        params Func<Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        CancellationToken cancellationToken,
        params Func<Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: examples
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectSomeAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] doWhenSome)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Perform a side effect on an option type when the inner value is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// void noneAction() => message = "None";
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This doesn't do anything since the input is Some.
    ///     .EffectNoneAsync(noneAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This sets message to "None" since the input is Some.
    ///     .EffectNoneAsync(noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option if it were some.</typeparam>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectNoneAsync<T>(
        this Task<Option<T>> optional,
        params Action[] doWhenNone)
        where T : notnull =>
            (await optional)
                .EffectNone(doWhenNone);

    // todo: example
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        params Action[] doWhenNone)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: example
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
        this Task<Option<T>> optional,
        CancellationToken cancellationToken,
        params Action[] doWhenNone)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: example
    // todo: docs
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
        this Task<Option<T>> optional,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] doWhenNone)
        where T : notnull
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Perform a side effect on an option type when the inner value is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string message = "";
    /// Task noneAction()
    /// {
    ///     message = "None";
    ///     return Task.CompletedTask;
    /// }
    /// 
    /// await Some("123")
    ///     .Async()
    ///     // This doesn't do anything since the input is Some.
    ///     .EffectNoneAsync(noneAction);
    ///     
    /// await None&lt;string&gt;()
    ///     .Async()
    ///     // This sets message to "None" since the input is Some.
    ///     .EffectNoneAsync(noneAction);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option if it were some.</typeparam>
    /// <param name="optional">The option to perform the side effect on.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectNoneAsync<T>(
        this Task<Option<T>> optional,
        params Func<Task>[] doWhenNone)
        where T : notnull
    {
        var option = await optional;
        return option.IsNone
            ? await RunSequential(doWhenNone)
            : Unit();
    }

    // todo: docs
    // todo: example
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
    this Task<Option<T>> optional,
    ProcessingOrder processingOrder,
    params Func<Task>[] doWhenNone)
    where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: docs
    // todo: example
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
    this Task<Option<T>> optional,
    CancellationToken cancellationToken,
    params Func<Task>[] doWhenNone)
    where T : notnull
    {
        throw new NotImplementedException();
    }

    // todo: docs
    // todo: example
    // todo: implement
    // todo: test
    public static async Task<Unit> EffectNoneAsync<T>(
    this Task<Option<T>> optional,
    ProcessingOrder processingOrder,
    CancellationToken cancellationToken,
    params Func<Task>[] doWhenNone)
    where T : notnull
    {
        throw new NotImplementedException();
    }
}