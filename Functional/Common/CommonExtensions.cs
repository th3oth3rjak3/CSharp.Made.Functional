﻿namespace Functional.Common;

/// <summary>
/// Extensions to improve the pipelining capability of regular
/// C# code.
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    /// Creates an immutable list of elements from inputs of the same type.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="items">The items to put into the list.</param>
    /// <returns>An immutable list of <typeparamref name="T"/></returns>
    public static ImmutableList<T> Cons<T>(params T[] items) =>
        ImmutableList<T>
            .Empty
            .AddRange(items);

    /// <summary>
    /// Ignores the output of a function. C# does this by default in functions
    /// that return void, but this can be used to declare that the output is
    /// intentionally ignored.
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="_">This parameter is ignored.</param>
    [ExcludeFromCodeCoverage]
    public static void Ignore<T>(this T? _) { }

    /// <summary>
    /// Ignores the output of an async function. C# does this by default in functions
    /// that return void, but this can be used to declare that the output is
    /// intentionally ignored.
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="toIgnore">This parameter is ignored.</param>
    public static async Task IgnoreAsync<T>(this Task<T> toIgnore) =>
        await toIgnore;

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns></returns>
    public static T Tap<T>(this T input, params Action<T>[] actions)
    {
        actions
            .ToList()
            .ForEach(action =>
                action(input));

        return input;
    }

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns></returns>
    public static T Tap<T>(this T input, params Action[] actions)
    {
        actions
            .ToList()
            .ForEach(action =>
                action());

        return input;
    }

    /// <summary>
    /// Map one object type to another using a mapping function.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The type to convert to.</typeparam>
    /// <param name="input">The input object.</param>
    /// <param name="mapper">A function delegate which transforms the 
    /// input type to the output type.</param>
    /// <returns>The mapped result.</returns>
    public static TResult Pipe<T, TResult>(this T input, Func<T, TResult> mapper) =>
        mapper(input);

    /// <summary>
    /// Perform a series of actions on the input and return unit.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="actions">Actions to be performed.</param>
    /// <returns>Unit.</returns>
    public static Unit Pipe<T>(this T input, params Action<T>[] actions) =>
        input
            .Tap(actions)
            .Pipe(_ => Unit.Default);

    /// <summary>
    /// Perform a series of actions while ignoring the input.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Pipe<T>(this T input, params Action[] actions) =>
        input.Tap(actions).Pipe(_ => Unit.Default);

    /// <summary>
    /// Perform a series of actions on the input and return unit.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="mapper">A mapping function with no inputs.</param>
    /// <returns>Unit.</returns>
    public static TOutput Pipe<T, TOutput>(this T input, Func<TOutput> mapper) =>
        input.Pipe(_ => mapper());

    /// <summary>
    /// Perform effects on any input type that returns unit.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="actions">Actions to perform on the input value.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action<T>[] actions) =>
        input.Pipe(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action[] actions) =>
        input.Pipe(actions);

    /// <summary>
    /// Perform an effect that returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect(Action action)
    {
        action();
        return Unit.Default;
    }

    /// <summary>
    /// Perform effects on the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Action<T>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Action[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects on the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Func<T, Task>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Func<Task>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform an effect which returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(Action action) =>
        await Effect(action).AsAsync();

    /// <summary>
    /// Perform an effect which returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(Func<Task> action)
    {
        await action();
        return Unit.Default;
    }

    /// <summary>
    /// Wraps an object with a ValueTask to be used with Async functions.
    /// </summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <param name="input">The input to be wrapped as a Task.</param>
    /// <returns>A Task of type <typeparamref name="T"/>.</returns>
    public static Task<T> AsAsync<T>(this T input) =>
        Task.FromResult(input);

    /// <summary>
    /// Used to wrap an action function that is asynchronous.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="input"/>.</typeparam>
    /// <param name="input">The input to use with the <paramref name="actions"/>.</param>
    /// <param name="actions">The action to await and call on the <paramref name="input"/>.</param>
    /// <returns>The resulting input as a task.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Func<T, Task>[] actions)
    {
        await Task.WhenAll(
            actions
                .Select(async action =>
                    await action(await input)));

        return await input;
    }

    /// <summary>
    /// Used to wrap an action that is asynchronous.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input used in the async action.</param>
    /// <param name="actions">The action to perform on the input.</param>
    /// <typeparam name="TResult">The resulting type after performing actions on the input.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<T> TapAsync<T, TResult>(
        this Task<T> input,
        params Func<T, Task<TResult>>[] actions)
    {
        await Task
            .WhenAll(
                actions
                    .Select(async action =>
                        await action(await input)))
            .IgnoreAsync();

        return await input;
    }

    /// <summary>
    /// Used to perform an action which returns void on an input that is a Task.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="input">The input to be awaited and then acted upon.</param>
    /// <param name="actions">The action to perform after awaiting the input.</param>
    /// <returns>The input as a task.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Action<T>[] actions) =>
        (await input)
            .Tap(actions);

    /// <summary>
    /// Used to wrap an async function that transforms a Task of <typeparamref name="TInput"/> to <typeparamref name="TInput"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="input">The input to transform.</param>
    /// <param name="func">The transformation function.</param>
    /// <returns>A result of the function as a task.</returns>
    public static async Task<TResult> PipeAsync<TInput, TResult>(this Task<TInput> input, Func<TInput, Task<TResult>> func) =>
        await func(await input);

    /// <summary>
    /// Used to wrap an async mapping function that transforms 
    /// <typeparamref name="TInput"/> to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TResult">The resulting type.</typeparam>
    /// <param name="input">The input to transform.</param>
    /// <param name="func">The transformation function.</param>
    /// <returns>The result of the transformation function.</returns>
    public static async Task<TResult> PipeAsync<TInput, TResult>(
        this Task<TInput> input,
        Func<TInput, TResult> func) =>
            func(await input);

    /// <summary>
    /// Used to wrap an async input that performs actions on the awaited input.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">A series of actions to perform on the input.</param>
    /// <returns>An awaitable Unit.</returns>
    public static async Task<Unit> PipeAsync<TInput>(this Task<TInput> input, params Action<TInput>[] actions) =>
        await (await input)
            .Tap(actions)
            .Pipe(_ => Unit.Default)
            .AsAsync();

    /// <summary>
    /// Used to wrap an async input that performs async actions on the awaited input.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">A series of actions to perform on the input.</param>
    /// <returns>An awaitable Unit.</returns>
    public static async Task<Unit> PipeAsync<TInput>(this Task<TInput> input, params Func<TInput, Task>[] actions) =>
        await (await input)
            .AsAsync()
            .TapAsync(actions)
            .PipeAsync(_ => Unit.Default);

    /// <summary>
    /// Used to wrap an async input that performs actions.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">A series of actions to perform on the input.</param>
    /// <returns>An awaitable Unit.</returns>
    public static async Task<Unit> PipeAsync<TInput>(this Task<TInput> input, params Action[] actions) =>
        await (await input)
            .Tap(actions)
            .AsAsync()
            .PipeAsync(_ => Unit.Default);

    /// <summary>
    /// Used to perform actions that return only a task.
    /// </summary>
    /// <typeparam name="TInput">The type of input that is ignored.</typeparam>
    /// <param name="input">The ignored input.</param>
    /// <param name="actions">Actions to be performed which result in a task.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> PipeAsync<TInput>(this Task<TInput> input, params Func<Task>[] actions)
    {
        await input;
        actions.ToList().ForEach(async action => await action());
        return Unit.Default;
    }

    /// <summary>
    /// Used to wrap an async input that performs actions.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="mapper">A mapping function that ignores the input from the pipeline.</param>
    /// <returns>An awaitable Unit.</returns>
    public static async Task<TOutput> PipeAsync<TInput, TOutput>(this Task<TInput> input, Func<TOutput> mapper) =>
        await (await input)
            .Pipe(_ => mapper())
            .AsAsync();

}
