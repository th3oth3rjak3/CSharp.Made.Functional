namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Create an Option that represents some value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; optional = Some("Hello, world!");
    /// Assert.IsTrue(optional.IsSome);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the inner content.</typeparam>
    /// <param name="entity">The contents to store.</param>
    /// <returns>A new Option with some data inside.</returns>
    public static Option<T> Some<T>(T entity) =>
        new(entity);

    /// <summary>
    /// Create an Option that represents no value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; optional = None&lt;string&gt;();
    /// Assert.IsTrue(optional.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the contents if they had been present.</typeparam>
    /// <returns>A new Option that represents a lack of contents.</returns>
    public static Option<T> None<T>() =>
        new();
}