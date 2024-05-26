﻿namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             value => someValue = value.ToString(),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             value => someValue = value.ToString(),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Action<T> whenSome, 
        Action whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome(theOption.Unwrap());
        if (theOption.IsNone) whenNone();

        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             value => someValue = value.ToString(),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             value => someValue = value.ToString(),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Action<T> whenSome, 
        Func<Task> whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome(theOption.Unwrap());
        if (theOption.IsNone) await whenNone();

        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             () => someValue = "Some",
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             () => someValue = "Some",
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Action whenSome, 
        Action whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome();
        if (theOption.IsNone) whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             () => someValue = "Some",
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             () => someValue = "Some",
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Action whenSome, 
        Func<Task> whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome();
        if (theOption.IsNone) await whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             value => EffectAsync(() => someValue = value.ToString()),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             value => EffectAsync(() => someValue = value.ToString()),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Func<T, Task> whenSome, 
        Action whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome(theOption.Unwrap());
        if (theOption.IsNone) whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             value => EffectAsync(() => someValue = value.ToString()),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             value => EffectAsync(() => someValue = value.ToString()),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Func<T, Task> whenSome, 
        Func<Task> whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome(theOption.Unwrap());
        if (theOption.IsNone) await whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             () => EffectAsync(() => someValue = "Some"),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             () => EffectAsync(() => someValue = "Some"),
    ///             () => noneValue = "None");
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Func<Task> whenSome, 
        Action whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome();
        if (theOption.IsNone) whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapAsync(
    ///             () => EffectAsync(() => someValue = "Some"),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// someValue = string.Empty;
    /// noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapAsync(
    ///             () => EffectAsync(() => someValue = "Some"),
    ///             () => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(
        this Task<Option<T>> optional, 
        Func<Task> whenSome, 
        Func<Task> whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome();
        if (theOption.IsNone) await whenNone();
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapSomeAsync(value => someValue = value.ToString());
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapSomeAsync(value => someValue = value.ToString());
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(
        this Task<Option<T>> optional, 
        params Action<T>[] whenSome) 
        where T : notnull =>
        (await optional)
            .TapSome(whenSome);
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapSomeAsync(() => someValue = "Some");
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapSomeAsync(() => someValue = "Some");
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(
        this Task<Option<T>> optional, 
        params Action[] whenSome) 
        where T : notnull =>
        (await optional)
            .TapSome(whenSome);
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapSomeAsync(value => EffectAsync(() => someValue = value.ToString()));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapSomeAsync(value => EffectAsync(() => someValue = value.ToString()));
    /// 
    /// Assert.AreEqual(someValue, "123");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(
        this Task<Option<T>> optional, 
        params Func<T, Task>[] whenSome) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return theOption;
        await RunSequential(theOption.Unwrap(), whenSome);
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = string.Empty;
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapSomeAsync(() => EffectAsync(() => someValue = "Some"));
    /// 
    /// Assert.AreEqual(someValue, string.Empty);
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapSomeAsync(() => EffectAsync(() => someValue = "Some"));
    /// 
    /// Assert.AreEqual(someValue, "Some");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(
        this Task<Option<T>> optional, 
        params Func<Task>[] whenSome) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsNone) return theOption;
        await RunSequential(whenSome);
        return theOption;
    }
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapNoneAsync(() => noneValue = "None");
    /// 
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapNoneAsync(() => noneValue = "None");
    /// 
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapNoneAsync<T>(
        this Task<Option<T>> optional, 
        params Action[] whenNone) 
        where T : notnull =>
        (await optional)
            .TapNone(whenNone);
    
    /// <summary>
    /// Tap into an option and perform a side effect without consuming the option when the option is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string noneValue = string.Empty;
    /// 
    /// Option&lt;int&gt; some =
    ///     await Some(123)
    ///         .Async()
    ///         .TapNoneAsync(() => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(noneValue, string.Empty);
    /// 
    /// Option&lt;int&gt; none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .TapNoneAsync(() => EffectAsync(() => noneValue = "None"));
    /// 
    /// Assert.AreEqual(noneValue, "None");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapNoneAsync<T>(
        this Task<Option<T>> optional, 
        params Func<Task>[] whenNone) 
        where T : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return theOption;
        await RunSequential(whenNone);
        return theOption;
    }

}