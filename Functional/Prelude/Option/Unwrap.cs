namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. If an option is None, it will throw an <see cref="InvalidOperationException"/>
    /// <br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Option contains some value using 
    /// <see cref="Option&lt;T&gt;.IsSome"/> or <see cref="Option&lt;T&gt;.IsNone"/>.
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional">The option to unwrap.</param>
    /// <returns>The inner value of the Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping a None.</exception>
    public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> optional)
    {
        var theOption = await optional;

        if (theOption.IsNone) throw new InvalidOperationException("Failed to unwrap the option because it was None. Be sure to check the Option by using the IsSome method before unwrapping.");

        return theOption.Unwrap();
    }
}
