namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) where T : notnull =>
        option
            .Match(
                some =>
                    predicate(some) switch
                    {
                        true => option,
                        false => None<T>()
                    },
                () => option);

    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> option, Func<T, bool> predicate) where T : notnull =>
        (await option)
            .Filter(predicate);
}