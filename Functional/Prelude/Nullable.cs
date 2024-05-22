namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Convert a non-nullable type to a nullable type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string nonNullableValue = "hello, world!";
    /// string? nullableValue = nonNullableValue.AsNullable();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type to make nullable.</typeparam>
    /// <param name="value">The value to make nullable.</param>
    /// <returns>The input value typed as nullable.</returns>
    public static T? AsNullable<T>(this T value) where T : notnull => value;

    /// <summary>
    /// Convert a non-nullable type to a nullable type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; nonNullableValue = Task.FromResult("hello, world!");
    /// Task&lt;string?&gt; nullableValue = nonNullableValue.AsNullable();
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type to make nullable.</typeparam>
    /// <param name="value">The value to make nullable.</param>
    /// <returns>The input value typed as nullable.</returns>
    public static async Task<T?> AsNullable<T>(this Task<T> value) where T : notnull =>
        await value;
}