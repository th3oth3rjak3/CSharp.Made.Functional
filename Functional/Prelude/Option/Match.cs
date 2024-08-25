namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string whenSome(int input) => input.ToString();
    /// string whenNone() => "none";
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "42");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return whenSome(theOption.Unwrap());
        return whenNone();
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; whenSome(int input) => Task.FromResult(input.ToString());
    /// string whenNone() => "none";
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "42");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return whenNone();
        return await whenSome(theOption.Unwrap());
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string whenSome(int input) => input.ToString();
    /// Task&lt;string&gt; whenNone() => Task.FromResult("none");
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "42");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return await whenNone();
        return whenSome(theOption.Unwrap());
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; whenSome(int input) => Task.FromResult(input.ToString());
    /// Task&lt;string&gt; whenNone() => Task.FromResult("none");
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "42");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return await whenNone();
        return await whenSome(theOption.Unwrap());
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string whenSome() => "some";
    /// string whenNone() => "none";
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "some");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TResult> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull =>
            (await optional).Match(whenSome, whenNone);

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; whenSome() => Task.FromResult("some");
    /// string whenNone() => "none";
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "some");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<Task<TResult>> whenSome,
        Func<TResult> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return await whenSome();
        return whenNone();
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string whenSome() => "some";
    /// Task&lt;string&gt; whenNone() => Task.FromResult("none");
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "some");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TResult> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return whenSome();
        return await whenNone();
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; whenSome() => Task.FromResult("some");
    /// Task&lt;string&gt; whenNone() => Task.FromResult("none");
    /// 
    /// string someResult = 
    ///     await Some(42)
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(someResult, "some");
    /// 
    /// string noneResult = 
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .MatchAsync(whenSome, whenNone);
    ///         
    /// Assert.AreEqual(noneResult, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<Task<TResult>> whenSome,
        Func<Task<TResult>> whenNone)
        where TInput : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return await whenSome();
        return await whenNone();
    }
}
