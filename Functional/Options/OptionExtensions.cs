namespace Functional.Options;

/// <summary>
/// Extensions to improve the functionality of the Option type.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Optional<T>(this T? entity) =>
        entity switch
        {
            null => Option.None<T>(),
            _ => entity.Some()
        };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Option<T> Optional<T>(this T? entity) where T : struct =>
        entity.HasValue switch
        {
            true => entity.Value.Some(),
            false => Option.None<T>(),
        };

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this Task<T?> entity)
    {
        var result = await entity;
        return result.Optional();
    }

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity) where T : class
    {
        var result = await entity;
        return result.Optional();
    }

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity) where T : struct
    {
        var result = await entity;
        return result.Optional();
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
    /// Option type other than Some or None.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<TResult> whenNone)
    {
        var option = await optional;

        return option.IsNone
            ? whenNone()
            // value will be non-null because the union was some.
            : whenSome(option.Unwrap()!);
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
    /// Option type other than Some or None.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<TResult> whenNone)
    {
        var result = await optional;

        if (result.IsNone) return whenNone();

        // value will be non-null because the union was some.
        return await whenSome(result.Unwrap()!);
    }
    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
    /// Option type other than Some or None.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, TResult> whenSome,
        Func<Task<TResult>> whenNone)
    {
        var result = await optional;

        if (result.IsNone) return await whenNone();

        // value will be non-null because the union was some.
        return whenSome(result.Unwrap()!);
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="optional">The option to be matched.</param>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    /// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
    /// Option type other than Some or None.</exception>
    public static async Task<TResult> MatchAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<TResult>> whenSome,
        Func<Task<TResult>> whenNone)
    {
        var result = await optional;

        if (result.IsNone) return await whenNone();

        // value will be non-null because the union was some.
        return await whenSome(result.Unwrap()!);
    }

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static Option<TResult> Map<T, TResult>(
        this Option<T> option,
        Func<T, TResult> mapper) =>
        option
            .Match(
                some => mapper(some).Optional(),
                Option.None<TResult>);

    /// <summary>
    /// When an Option is Some, map to a new value, ignoring the old value.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static Option<TResult> Map<T, TResult>(
        this Option<T> option,
        Func<TResult> mapper) =>
        option
            .Match(
                _ => mapper().Optional(),
                Option.None<TResult>);

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> option,
        Func<T, TResult> mapper)
    {
        var result = await option;
        return result.Map(mapper);
    }

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> option,
        Func<T, Task<TResult>> mapper)
    {
        var result = await option;

        if (result.IsNone) return Option.None<TResult>();

        var contents = result.Unwrap()!;

        // value will be non-null because the union was some.
        return (await mapper(contents))
            .Optional();

    }

    /// <summary>
    /// When an Option is Some, map the existing
    /// value to a new type with a provided function.
    /// </summary>
    /// <typeparam name="T">The original type.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="option">The option to be mapped.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Option<Task<T>> option,
        Func<T, TResult> mapper)
    {
        if (option.IsNone) return Option.None<TResult>();

        // value will be non-null because the union was some.
        var contents = await option.Unwrap()!;

        return mapper(contents).Optional();
    }

    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) =>
        option
            .Match(
                some =>
                    predicate(some) switch
                    {
                        true => option,
                        false => Option.None<T>()
                    },
                () => option);

    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> option, Func<T, bool> predicate) =>
        (await option)
            .Filter(predicate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static T Reduce<T>(this Option<T> optional, Func<T> alternate) =>
        optional
            .Match(
                some => some,
                alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static T Reduce<T>(this Option<T> optional, T alternate) =>
        optional
            .Match(
                some => some,
                () => alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, Func<T> alternate)
    {
        var result = await optional;

        return result.Match(
            some => some,
            alternate);
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        T alternate)
    {
        var result = await optional;

        return result.Match(
            some => some,
            () => alternate);
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        Task<T> alternate)
    {
        var result = await optional;

        // value will be non-null because the union was some.
        return result.IsSome ? result.Unwrap()! : await alternate;
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        Func<Task<T>> alternate)
    {
        var result = await optional;

        // value will be non-null because the union was some.
        return result.IsSome ? result.Unwrap()! : await alternate();
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Option<Task<T>> optional,
        T alternate)
    {
        if (optional.IsNone) return alternate;

        // value will be non-null because the union was some.
        return await optional.Unwrap()!;
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Option<Task<T>> optional,
        Func<T> alternate)
    {
        if (optional.IsNone) return alternate();

        // value will be non-null because the union was some.
        return await optional.Unwrap()!;
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Option<Task<T>> optional,
        Task<T> alternate)
    {
        if (optional.IsNone) return await alternate;

        // value will be non-null because the union was some.
        return await optional.Unwrap()!;
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise return the alternate value when None.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Option<Task<T>> optional,
        Func<Task<T>> alternate)
    {
        if (optional.IsNone) return await alternate();

        // value will be non-null because the union was some.
        return await optional.Unwrap()!;
    }

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static Option<TResult> Bind<TInput, TResult>(
        this Option<TInput> optional,
        Func<TInput, Option<TResult>> binder) =>
            optional
                .Map(binder)
                .Reduce(Option.None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Option<TResult>> binder) =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(() => Option.None<TResult>());

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<Option<TResult>>> binder) =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(() => Option.None<TResult>());

    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. If an option is None, it will return null.
    /// <br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Option contains some value using 
    /// <see cref="Option&lt;T&gt;.IsSome"/> or <see cref="Option&lt;T&gt;.IsNone"/>.
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional">The option to unwrap.</param>
    /// <returns>The inner value of the Option.</returns>
    public static async Task<T?> UnwrapAsync<T>(this Task<Option<T>> optional) =>
        (await optional).Unwrap();

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome, Action doWhenNone) =>
        (await optional)
            .Effect(doWhenSome, doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action doWhenSome, Action doWhenNone) =>
        (await optional)
            .Effect(_ => doWhenSome(), doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome) =>
        (await optional)
            .Effect(doWhenSome, () => { });

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Action doWhenSome) =>
        (await optional)
            .Effect(_ => doWhenSome(), () => { });

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Func<T, Task> doWhenSome)
    {
        var option = await optional;
        if (option.IsSome)
        {
            await doWhenSome(option.Unwrap()!);
        }

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenSome)
    {
        var option = await optional;
        if (option.IsSome)
        {
            await doWhenSome();
        }

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is None.
    /// </summary>
    /// <typeparam name="T">The type of the option if it were some.</typeparam>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectNoneAsync<T>(this Task<Option<T>> optional, Action doWhenNone) =>
        (await optional)
            .Effect(_ => { }, doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is None.
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional"></param>
    /// <param name="doWhenNone"></param>
    /// <returns></returns>
    public static async Task<Unit> EffectNoneAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenNone)
    {
        var option = await optional;
        if (option.IsNone) await doWhenNone();

        return Unit.Default;
    }
}
