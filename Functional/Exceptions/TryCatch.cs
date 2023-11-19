namespace Functional.Exceptions;

/// <summary>
/// Handle Try/Catch/Finally with Fluent Syntax.
/// </summary>
public static class TryCatch
{
    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A TryResult which is used by <see cref="Catch{TResult}(TryResult{TResult}, Func{Exception, TResult})"/> through deferred execution.</returns>
    public static TryResult<TResult> Try<TResult>(Func<TResult> toTry) =>
        new(toTry);

    /// <summary>
    /// Catch the TryResult with a handler to be used during deferred execution.
    /// </summary>
    /// <typeparam name="TResult">The return type of the TryResult and the CatchHandler.</typeparam>
    /// <param name="tried">The TryResult to be used later.</param>
    /// <param name="catchHandler">The function to execute when an exception is thrown.</param>
    /// <returns>A CatchResult to be used with <see cref="Invoke{TResult}(CatchResult{TResult})"/> or 
    /// <see cref="Finally{TResult}(CatchResult{TResult}, Action)"/></returns>
    public static CatchResult<TResult> Catch<TResult>(this TryResult<TResult> tried, Func<Exception, TResult> catchHandler) =>
        new(tried, catchHandler);

    /// <summary>
    /// Perform the Try/Catch operation.
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch operation.</returns>
    public static TResult Invoke<TResult>(this CatchResult<TResult> caught)
    {
        try
        {
            return caught.Tried.TryAction();
        }
        catch (Exception ex)
        {
            return caught.CatchHandler(ex);
        }
    }

    /// <summary>
    /// Add a finally condition to the Try/Catch operation.
    /// </summary>
    /// <typeparam name="TResult">The return type of the TryResult TryAction and the CatchHandler.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <param name="finallyAction">An action to perform in the Finally block.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static FinallyResult<TResult> Finally<TResult>(this CatchResult<TResult> caught, Action finallyAction) =>
        new(caught, finallyAction);

    /// <summary>
    /// Perform the Try/Catch/Finally operation.
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="finallyResult">The FinallyResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static TResult Invoke<TResult>(this FinallyResult<TResult> finallyResult)
    {
        try
        {
            return finallyResult.Caught.Tried.TryAction();
        }
        catch (Exception ex)
        {
            return finallyResult.Caught.CatchHandler(ex);
        }
        finally
        {
            finallyResult.Finally();
        }
    }

    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="T">The input to be used in the toTry function.</typeparam>
    /// <param name="input">The input value to be used in the toTry function.</param>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A TryResult which is used by <see cref="Catch{T, TResult}(TryInputResult{T, TResult}, Func{Exception, TResult})"/> through deferred execution.</returns>
    public static TryInputResult<T, TResult> Try<T, TResult>(this T input, Func<T, TResult> toTry) =>
        new(input, toTry);

    /// <summary>
    /// Catch the TryInputResult with a handler to be used during deferred execution.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The return type of the TryInputResult and the CatchHandler.</typeparam>
    /// <param name="tried">The TryInputResult to be used later.</param>
    /// <param name="catchHandler">The function to execute when an exception is thrown.</param>
    /// <returns>A CatchInputResult to be used with <see cref="Invoke{T, TResult}(CatchInputResult{T, TResult})"/> or 
    /// <see cref="Finally{T, TResult}(CatchInputResult{T, TResult}, Action)"/></returns>
    public static CatchInputResult<T, TResult> Catch<T, TResult>(this TryInputResult<T, TResult> tried, Func<Exception, TResult> catchHandler) =>
        new(tried, catchHandler);

    /// <summary>
    /// Perform the Try/Catch operation.
    /// </summary>
    /// <typeparam name="T">The input type of the CatchInputResult.</typeparam>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="caught">The CatchInputResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch operation.</returns>
    public static TResult Invoke<T, TResult>(this CatchInputResult<T, TResult> caught)
    {
        try
        {
            return caught.Tried.TryAction(caught.Tried.Value);
        }
        catch (Exception ex)
        {
            return caught.CatchHandler(ex);
        }
    }

    /// <summary>
    /// Perform the Try/Catch operation with a finally block.
    /// </summary>
    /// <typeparam name="T">The input type of the CatchInputResult.</typeparam>
    /// <typeparam name="TResult">The return type of the TryResult TryAction and the CatchHandler.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <param name="finallyAction">An action to perform in the Finally block.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static FinallyInputResult<T, TResult> Finally<T, TResult>(this CatchInputResult<T, TResult> caught, Action finallyAction) =>
        new(caught, finallyAction);

    /// <summary>
    /// Perform the Try/Catch/Finally operation.
    /// </summary>
    /// <typeparam name="T">The input type of the FinallyInputResult.</typeparam>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="finallyResult">The FinallyInputResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static TResult Invoke<T, TResult>(this FinallyInputResult<T, TResult> finallyResult)
    {
        try
        {
            return finallyResult.Caught.Tried.TryAction(finallyResult.Caught.Tried.Value);
        }
        catch (Exception ex)
        {
            return finallyResult.Caught.CatchHandler(ex);
        }
        finally
        {
            finallyResult.Finally();
        }
    }

    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A TryResult which is used by <see cref="Catch{TResult}(TryResult{TResult}, Func{Exception, TResult})"/> through deferred execution.</returns>
    public static TryResultAsync<TResult> TryAsync<TResult>(Func<Task<TResult>> toTry) =>
        new(toTry);

    /// <summary>
    /// Catch the TryResult with a handler to be used during deferred execution.
    /// </summary>
    /// <typeparam name="TResult">The return type of the TryResult and the CatchHandler.</typeparam>
    /// <param name="tried">The TryResult to be used later.</param>
    /// <param name="catchHandler">The function to execute when an exception is thrown.</param>
    /// <returns>A CatchResult to be used with <see cref="Invoke{TResult}(CatchResult{TResult})"/> or 
    /// <see cref="Finally{TResult}(CatchResult{TResult}, Action)"/></returns>
    public static CatchResultAsync<TResult> CatchAsync<TResult>(this TryResultAsync<TResult> tried, Func<Exception, Task<TResult>> catchHandler) =>
        new(tried, catchHandler);

    /// <summary>
    /// Perform the Try/Catch operation.
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch operation.</returns>
    public static async Task<TResult> InvokeAsync<TResult>(this CatchResultAsync<TResult> caught)
    {
        try
        {
            return await caught.Tried.TryAction();
        }
        catch (Exception ex)
        {
            return await caught.CatchHandler(ex);
        }
    }

    /// <summary>
    /// Add a finally condition to the Try/Catch operation.
    /// </summary>
    /// <typeparam name="TResult">The return type of the TryResult TryAction and the CatchHandler.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <param name="finallyAction">An action to perform in the Finally block.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static FinallyResultAsync<TResult> FinallyAsync<TResult>(this CatchResultAsync<TResult> caught, Action finallyAction) =>
        new(caught, finallyAction);

    /// <summary>
    /// Perform the Try/Catch/Finally operation.
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="finallyResult">The FinallyResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static async Task<TResult> InvokeAsync<TResult>(this FinallyResultAsync<TResult> finallyResult)
    {
        try
        {
            return await finallyResult.Caught.Tried.TryAction();
        }
        catch (Exception ex)
        {
            return await finallyResult.Caught.CatchHandler(ex);
        }
        finally
        {
            finallyResult.Finally();
        }
    }

    /// <summary>
    /// Try something which could throw an exception.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <typeparam name="T">The input to be used in the toTry function.</typeparam>
    /// <param name="input">The input value to be used in the toTry function.</param>
    /// <param name="toTry">The function to try.</param>
    /// <returns>A TryResult which is used by <see cref="CatchAsync{T, TResult}(TryInputResultAsync{T, TResult}, Func{Exception, Task{TResult}})"/> through deferred execution.</returns>
    public static TryInputResultAsync<T, TResult> TryAsync<T, TResult>(this Task<T> input, Func<T, Task<TResult>> toTry) =>
        new(input, toTry);

    /// <summary>
    /// Catch the TryInputResult with a handler to be used during deferred execution.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The return type of the TryInputResult and the CatchHandler.</typeparam>
    /// <param name="tried">The TryInputResult to be used later.</param>
    /// <param name="catchHandler">The function to execute when an exception is thrown.</param>
    /// <returns>A CatchInputResult to be used with <see cref="Invoke{T, TResult}(CatchInputResult{T, TResult})"/> or 
    /// <see cref="Finally{T, TResult}(CatchInputResult{T, TResult}, Action)"/></returns>
    public static CatchInputResultAsync<T, TResult> CatchAsync<T, TResult>(this TryInputResultAsync<T, TResult> tried, Func<Exception, Task<TResult>> catchHandler) =>
        new(tried, catchHandler);

    /// <summary>
    /// Perform the Try/Catch operation.
    /// </summary>
    /// <typeparam name="T">The input type of the CatchInputResult.</typeparam>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="caught">The CatchInputResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch operation.</returns>
    public static async Task<TResult> InvokeAsync<T, TResult>(this CatchInputResultAsync<T, TResult> caught)
    {
        try
        {
            return await caught.Tried.TryAction(await caught.Tried.Value);
        }
        catch (Exception ex)
        {
            return await caught.CatchHandler(ex);
        }
    }

    /// <summary>
    /// Perform the Try/Catch operation with a finally block.
    /// </summary>
    /// <typeparam name="T">The input type of the CatchInputResult.</typeparam>
    /// <typeparam name="TResult">The return type of the TryResult TryAction and the CatchHandler.</typeparam>
    /// <param name="caught">The CatchResult to be handled in this function.</param>
    /// <param name="finallyAction">An action to perform in the Finally block.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static FinallyInputResultAsync<T, TResult> FinallyAsync<T, TResult>(this CatchInputResultAsync<T, TResult> caught, Action finallyAction) =>
        new(caught, finallyAction);

    /// <summary>
    /// Perform the Try/Catch/Finally operation.
    /// </summary>
    /// <typeparam name="T">The input type of the FinallyInputResult.</typeparam>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="finallyResult">The FinallyInputResult to be handled in this function.</param>
    /// <returns>The result of the Try/Catch/Finally operation.</returns>
    public static async Task<TResult> InvokeAsync<T, TResult>(this FinallyInputResultAsync<T, TResult> finallyResult)
    {
        try
        {
            return await finallyResult.Caught.Tried.TryAction(await finallyResult.Caught.Tried.Value);
        }
        catch (Exception ex)
        {
            return await finallyResult.Caught.CatchHandler(ex);
        }
        finally
        {
            finallyResult.Finally();
        }
    }
}

/// <summary>
/// A wrapper around something to try which could throw an exception.
/// </summary>
/// <typeparam name="TResult">The output type of trying the action.</typeparam>
/// <param name="TryAction">The action to try.</param>
public sealed record TryResult<TResult>(Func<TResult> TryAction);

/// <summary>
/// A wrapper around a TryResult with a function to handle caught exceptions.
/// </summary>
/// <typeparam name="TResult">The output type of catch handler.</typeparam>
/// <param name="Tried">The TryResult to wrap.</param>
/// <param name="CatchHandler">The function that handles an exception.</param>
public sealed record CatchResult<TResult>(TryResult<TResult> Tried, Func<Exception, TResult> CatchHandler);

/// <summary>
/// A wrapper around a CatchResult to add a finally action to the Try/Catch/Finally operation.
/// </summary>
/// <typeparam name="TResult">The type of the try and catch results.</typeparam>
/// <param name="Caught">The CatchResult to wrap.</param>
/// <param name="Finally">The action to perform in the finally block.</param>
public sealed record FinallyResult<TResult>(CatchResult<TResult> Caught, Action Finally);

/// <summary>
/// A wrapper around a function to try which takes input from a previous function.
/// </summary>
/// <typeparam name="T">The input value type.</typeparam>
/// <typeparam name="TResult">The type returned by performing the TryAction.</typeparam>
/// <param name="Value">The input value.</param>
/// <param name="TryAction">The action to perform on the input value.</param>
public sealed record TryInputResult<T, TResult>(T Value, Func<T, TResult> TryAction);

/// <summary>
/// A wrapper around a TryInputResult with a function to handle caught exceptions.
/// </summary>
/// <typeparam name="T">The input type for the TryInputResult.</typeparam>
/// <typeparam name="TResult">The output type for the catch handler.</typeparam>
/// <param name="Tried">The TryInputResult to wrap.</param>
/// <param name="CatchHandler">A function to execute when an exception is thrown.</param>
public sealed record CatchInputResult<T, TResult>(TryInputResult<T, TResult> Tried, Func<Exception, TResult> CatchHandler);

/// <summary>
/// A wrapper around a CatchInputResult to add a finally action to the Try/Catch/Finally operation.
/// </summary>
/// <typeparam name="T">The input type for the TryInputResult.</typeparam>
/// <typeparam name="TResult">The output type for the TryInputResult.TryAction and the CatchHandler.</typeparam>
/// <param name="Caught">The CatchResult to wrap.</param>
/// <param name="Finally">The action to perform in the finally block.</param>
public sealed record FinallyInputResult<T, TResult>(CatchInputResult<T, TResult> Caught, Action Finally);

/// <summary>
/// A wrapper around something to try which could throw an exception.
/// </summary>
/// <typeparam name="TResult">The output type of trying the action.</typeparam>
/// <param name="TryAction">The action to try.</param>
public sealed record TryResultAsync<TResult>(Func<Task<TResult>> TryAction);

/// <summary>
/// A wrapper around a TryResult with a function to handle caught exceptions.
/// </summary>
/// <typeparam name="TResult">The output type of catch handler.</typeparam>
/// <param name="Tried">The TryResult to wrap.</param>
/// <param name="CatchHandler">The function that handles an exception.</param>
public sealed record CatchResultAsync<TResult>(TryResultAsync<TResult> Tried, Func<Exception, Task<TResult>> CatchHandler);

/// <summary>
/// A wrapper around a CatchResult to add a finally action to the Try/Catch/Finally operation.
/// </summary>
/// <typeparam name="TResult">The type of the try and catch results.</typeparam>
/// <param name="Caught">The CatchResult to wrap.</param>
/// <param name="Finally">The action to perform in the finally block.</param>
public sealed record FinallyResultAsync<TResult>(CatchResultAsync<TResult> Caught, Action Finally);

/// <summary>
/// A wrapper around a function to try which takes input from a previous function.
/// </summary>
/// <typeparam name="T">The input value type.</typeparam>
/// <typeparam name="TResult">The type returned by performing the TryAction.</typeparam>
/// <param name="Value">The input value.</param>
/// <param name="TryAction">The action to perform on the input value.</param>
public sealed record TryInputResultAsync<T, TResult>(Task<T> Value, Func<T, Task<TResult>> TryAction);

/// <summary>
/// A wrapper around a TryInputResult with a function to handle caught exceptions.
/// </summary>
/// <typeparam name="T">The input type for the TryInputResult.</typeparam>
/// <typeparam name="TResult">The output type for the catch handler.</typeparam>
/// <param name="Tried">The TryInputResult to wrap.</param>
/// <param name="CatchHandler">A function to execute when an exception is thrown.</param>
public sealed record CatchInputResultAsync<T, TResult>(TryInputResultAsync<T, TResult> Tried, Func<Exception, Task<TResult>> CatchHandler);

/// <summary>
/// A wrapper around a CatchInputResult to add a finally action to the Try/Catch/Finally operation.
/// </summary>
/// <typeparam name="T">The input type for the TryInputResult.</typeparam>
/// <typeparam name="TResult">The output type for the TryInputResult.TryAction and the CatchHandler.</typeparam>
/// <param name="Caught">The CatchResult to wrap.</param>
/// <param name="Finally">The action to perform in the finally block.</param>
public sealed record FinallyInputResultAsync<T, TResult>(CatchInputResultAsync<T, TResult> Caught, Action Finally);
