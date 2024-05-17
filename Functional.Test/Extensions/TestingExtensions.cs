namespace Functional.Test.Extensions;

internal static class TestingExtensions
{
    /// <summary>
    /// Assert that an item is an instance of the provided type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This won't throw since it's a string.
    /// "string value".AssertInstanceOfType(typeof(string));
    /// 
    /// // This will throw since it's not an int.
    /// "string value".AssertInstanceOfType(typeof(int));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="instance">The instance to test against the provided type.</param>
    /// <param name="expectedType">A type that the instance should implement or inherit from.</param>
    internal static void AssertInstanceOfType<T>(this T instance, Type expectedType) =>
        Assert.IsInstanceOfType(instance, expectedType);
}
