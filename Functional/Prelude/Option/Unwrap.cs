namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Determine if an option has some value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// bool isSome =
    ///     await Some("123")
    ///         .Async()
    ///         .IsSome();
    ///         
    /// Assert.IsTrue(isSome);
    /// 
    /// isSome =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         .IsSome();
    ///         
    /// Assert.IsFalse(isSome);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional">The option to check for some value.</param>
    /// <returns>True when Some value, otherwise False.</returns>
    public static async Task<bool> IsSome<T>(this Task<Option<T>> optional) where T : notnull =>
        (await optional).IsSome;


    /// <summary>
    /// Determine if an option has no value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// bool isNone =
    ///     await Some("123")
    ///         .Async()
    ///         .IsNone();
    ///         
    /// Assert.IsFalse(isNone);
    /// 
    /// isNone =
    ///     await None&lt;string&gt;()
    ///         .Async()
    ///         .IsNone();
    ///         
    /// Assert.IsTrue(isNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional">The option to check for no value.</param>
    /// <returns>True when None, otherwise False.</returns>
    public static async Task<bool> IsNone<T>(this Task<Option<T>> optional) where T : notnull =>
        (await optional).IsNone;

    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. 
    /// <br/>If an option is None, it will throw an InvalidOperationException.
    /// <br/>For more information, see: <see cref="Option{T}.IsSome"/> or <see cref="Option{T}.IsNone"/>.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will be fine
    /// new Option&lt;string&gt;("hello, world!").Async().Unwrap();
    /// 
    /// // This will throw an InvalidOperationException
    /// new Option&lt;string&gt;(null as string).Async().Unwrap();
    /// 
    /// // To use this safely, perform the following steps
    /// Task&lt;Option&lt;string&gt;&gt; optional = new Option&lt;string&gt;(null as string).Async(); // This will be a None.
    /// 
    /// if (await optional.IsSome())
    /// {
    ///     var contents = await optional.Unwrap();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping a None.</exception>
    public static async Task<T> Unwrap<T>(this Task<Option<T>> optional) where T : notnull
    {
        var theOption = await optional;

        if (theOption.IsNone) throw new InvalidOperationException("Failed to unwrap the option because it was None. Be sure to check the Option by using the IsSome method before unwrapping.");

        return theOption.Unwrap();
    }
}