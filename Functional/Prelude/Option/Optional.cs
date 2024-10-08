﻿namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string? maybeString(int input) => 
    ///     input > 10 
    ///     ? input.ToString()
    ///     : null;
    ///     
    /// // Convert results of function calls to option from nullable.
    /// Option&lt;string&gt; optionalValue =
    ///     maybeString(42)
    ///         .Optional();
    ///         
    /// // Convert possible null parameters to options
    /// void DoWork(string? maybeValue)
    /// {
    ///     maybeValue
    ///         .Optional()
    ///         .EffectSome(value => Console.WriteLine(value));
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Optional<T>(this T? entity) =>
        (entity is null) switch
        {
            true => None<T>(),
            false => Some(entity)
        };

    /// <summary>
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int? maybeInt(int input) =>
    ///     input > 10
    ///     ? input
    ///     : null
    ///     
    /// // Convert results of function calls to option from nullable.
    /// Option&lt;int&gt; optionalValue =
    ///     maybeInt(42)
    ///         .Optional();
    ///         
    /// // Convert possible null parameters to options
    /// void DoWork(int? maybeValue)
    /// {
    ///     maybeValue
    ///         .Optional()
    ///         .EffectSome(value => Console.WriteLine(value.ToString()));
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Optional<T>(this T? entity) where T : struct =>
        entity.HasValue switch
        {
            true => entity.Value.Pipe(Some),
            false => None<T>()
        };

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// async Task&lt;string?&gt; getValueLater(int entityId)
    /// {
    ///     if (entityId > 10) 
    ///     {
    ///         await Task.Delay(1000);
    ///         return entityId.ToString();
    ///     }
    ///     
    ///     return null;
    /// }
    /// 
    /// // Converts a Task&lt;string?&gt; into a Task&lt;Option&lt;string&gt;&gt;
    /// Option&lt;string&gt; optional = 
    ///     await getValueLater(42)
    ///         .Optional();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this Task<T?> entity) =>
        (await entity).Optional();

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// async Task&lt;int?&gt; getValueLater(int entityId)
    /// {
    ///     if (entityId > 10) 
    ///     {
    ///         await Task.Delay(1000);
    ///         return entityId;
    ///     }
    ///     
    ///     return null;
    /// }
    /// 
    /// // Converts a Task&lt;int?&gt; into a Task&lt;Option&lt;int&gt;&gt;
    /// Option&lt;int&gt; optional = 
    ///     await getValueLater(42)
    ///         .Optional();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this Task<T?> entity) where T : struct
    {
        var result = await entity;
        return result.Optional();
    }

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// async ValueTask&lt;string?&gt; getValueLater(int entityId)
    /// {
    ///     if (entityId > 10) 
    ///     {
    ///         await Task.Delay(1000);
    ///         return ValueTask.FromResult(entityId.ToString());
    ///     }
    ///     
    ///     return ValueTask.FromResult(null as string);
    /// }
    /// 
    /// // Converts a ValueTask&lt;string?&gt; into a Task&lt;Option&lt;string&gt;&gt;
    /// Option&lt;string&gt; optional = 
    ///     await getValueLater(42)
    ///         .Optional();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity)
    {
        var result = await entity;
        return result.Optional();
    }

    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// async ValueTask&lt;int?&gt; getValueLater(int entityId)
    /// {
    ///     if (entityId > 10) 
    ///     {
    ///         await Task.Delay(1000);
    ///         return ValueTask.FromResult(entityId);
    ///     }
    ///     
    ///     return ValueTask.FromResult(null as int?);
    /// }
    /// 
    /// // Converts a ValueTask&lt;int?&gt; into a Task&lt;Option&lt;int&gt;&gt;
    /// Option&lt;int&gt; optional = 
    ///     await getValueLater(42)
    ///         .Optional();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity) where T : struct
    {
        var result = await entity;
        return result.Optional();
    }
}