namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Action<T> whenSome, Action whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome(theOption.Unwrap());
        if (theOption.IsNone) whenNone();

        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Action<T> whenSome, Func<Task> whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome(theOption.Unwrap());
        if (theOption.IsNone) await whenNone();

        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Action whenSome, Action whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome();
        if (theOption.IsNone) whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Action whenSome, Func<Task> whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) whenSome();
        if (theOption.IsNone) await whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Func<T, Task> whenSome, Action whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome(theOption.Unwrap());
        if (theOption.IsNone) whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Func<T, Task> whenSome, Func<Task> whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome(theOption.Unwrap());
        if (theOption.IsNone) await whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Func<Task> whenSome, Action whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome();
        if (theOption.IsNone) whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapAsync<T>(this Task<Option<T>> optional, Func<Task> whenSome, Func<Task> whenNone)
    {
        var theOption = await optional;
        if (theOption.IsSome) await whenSome();
        if (theOption.IsNone) await whenNone();
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is Some.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(this Task<Option<T>> optional, params Action<T>[] whenSome) =>
        (await optional)
            .TapSome(whenSome);

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is Some.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(this Task<Option<T>> optional, params Action[] whenSome) =>
        (await optional)
            .TapSome(whenSome);

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is Some.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(this Task<Option<T>> optional, params Func<T, Task>[] whenSome)
    {
        var theOption = await optional;
        if (theOption.IsSome)
        {
            var contents = theOption.Unwrap();
            whenSome.ToList().ForEach(async action => await action(contents));
        }
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is Some.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenSome">The action to perform when Some.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapSomeAsync<T>(this Task<Option<T>> optional, params Func<Task>[] whenSome)
    {
        var theOption = await optional;
        if (theOption.IsSome)
        {
            whenSome.ToList().ForEach(async action => await action());
        }
        return theOption;
    }

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is None.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapNoneAsync<T>(this Task<Option<T>> optional, params Action[] whenNone) =>
        (await optional)
            .TapNone(whenNone);

    /// <summary>
    /// Tap into an option and perform a side-effect without consuming the option when the option is None.
    /// </summary>
    /// <typeparam name="T">The type of the option when Some.</typeparam>
    /// <param name="optional">The option to tap.</param>
    /// <param name="whenNone">The action to perform when None.</param>
    /// <returns>The input option.</returns>
    public static async Task<Option<T>> TapNoneAsync<T>(this Task<Option<T>> optional, params Func<Task>[] whenNone)
    {
        var theOption = await optional;
        if (theOption.IsNone)
        {
            whenNone.ToList().ForEach(async action => await action());
        }
        return theOption;
    }

}
