namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Filters out strings with length greater than or equal to 10
    /// bool filterCriteria(string input) => input.Length &lt; 10;
    /// 
    /// Option&lt;string&gt; some = Some("short");
    /// 
    /// // Not filtered out because the value "short" matched the predicate.
    /// var filtered = await some.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsSome);
    /// 
    /// some = Some("a really long message that will get filtered out");
    /// 
    /// // Filtered out because the Length of the input was greater than 10 characters.
    /// filtered = await some.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// 
    /// Option&lt;string&gt; none = None&lt;string&gt;();
    /// 
    /// // Input was None, so it's still a None.
    /// filtered = await none.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the Option.</typeparam>
    /// <param name="optional">The input option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> optional, Func<T, bool> predicate) where T : notnull =>
        (await optional)
            .Filter(predicate);


    /// <summary>
    /// Convert a Some into a None when it doesn't match the provided predicate.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Filters out strings with length greater than or equal to 10
    /// TasK&lt;bool&gt; filterCriteria(string input) => Task.FromResult(input.Length &lt; 10);
    /// 
    /// Option&lt;string&gt; some = Some("short");
    /// 
    /// // Not filtered out because the value "short" matched the predicate.
    /// var filtered = await some.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsSome);
    /// 
    /// some = Some("a really long message that will get filtered out");
    /// 
    /// // Filtered out because the Length of the input was greater than 10 characters.
    /// filtered = await some.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// 
    /// Option&lt;string&gt; none = None&lt;string&gt;();
    /// 
    /// // Input was None, so it's still a None.
    /// filtered = await none.Async().FilterAsync(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the Option.</typeparam>
    /// <param name="optional">The input option to filter.</param>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> FilterAsync<T>(this Task<Option<T>> optional, Func<T, Task<bool>> predicate) where T : notnull
    {
        var theOption = await optional;

        if (theOption.IsNone) return theOption;

        if (theOption.IsSome && await predicate(theOption.Unwrap())) return theOption;

        return new();
    }

    /// <summary>
    /// Filter a collection of options based on a predicate, converting the unmatched values to None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Filters out strings with length greater than or equal to 10
    /// bool filterCriteria(string input) => input.Length &lt; 10;
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; collection = [ 
    ///     // Not filtered because it has fewer than 10 characters
    ///     Some("short"), 
    ///     // Ignored because it's already None.
    ///     None&lt;string&gt;(), 
    ///     // Filtered out because it's too long.
    ///     Some("Really long message to filter out") ];
    /// 
    /// List&lt;Option&lt;string&gt;&gt; filtered = 
    ///     collection
    ///         .Filter(filterCriteria)
    ///         .ToList();
    /// 
    /// Assert.AreEqual(filtered[0], Some("short"));
    /// Assert.AreEqual(filtered[1], None&lt;string&gt;());
    /// Assert.AreEqual(filtered[2], None&lt;string&gt;());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of each Option.</typeparam>
    /// <param name="collection">The collection to filter.</param>
    /// <param name="predicate">The predicate filter criteria.</param>
    /// <returns>The collection with unmatched items converted to None.</returns>
    public static IEnumerable<Option<T>> Filter<T>(this IEnumerable<Option<T>> collection, Func<T, bool> predicate) where T : notnull =>
        collection.Select(option => option.Filter(predicate));

    /// <summary>
    /// Filter a collection of options based on a predicate, converting the unmatched values to None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Filters out strings with length greater than or equal to 10
    /// bool filterCriteria(string input) => input.Length &lt; 10;
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; collection = [ 
    ///     // Not filtered because it has fewer than 10 characters
    ///     Some("short"), 
    ///     // Ignored because it's already None.
    ///     None&lt;string&gt;(), 
    ///     // Filtered out because it's too long.
    ///     Some("Really long message to filter out") ];
    /// 
    /// List&lt;Option&lt;string&gt;&gt; filtered = 
    ///     await collection
    ///         .Async()
    ///         .FilterAsync(filterCriteria)
    ///         .PipeAsync(values => values.ToList());
    /// 
    /// Assert.AreEqual(filtered[0], Some("short"));
    /// Assert.AreEqual(filtered[1], None&lt;string&gt;());
    /// Assert.AreEqual(filtered[2], None&lt;string&gt;());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of each Option.</typeparam>
    /// <param name="collection">The collection to filter.</param>
    /// <param name="predicate">The predicate filter criteria.</param>
    /// <returns>The collection with unmatched items converted to None.</returns>
    public static async Task<IEnumerable<Option<T>>> FilterAsync<T>(this Task<IEnumerable<Option<T>>> collection, Func<T, bool> predicate) where T : notnull =>
        (await collection).Filter(predicate);

    /// <summary>
    /// Filter a collection of options based on a predicate, converting the unmatched values to None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Filters out strings with length greater than or equal to 10
    /// Task&lt;bool&gt; filterCriteria(string input) => Task.FromResult(input.Length &lt; 10);
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; collection = [ 
    ///     // Not filtered because it has fewer than 10 characters
    ///     Some("short"), 
    ///     // Ignored because it's already None.
    ///     None&lt;string&gt;(), 
    ///     // Filtered out because it's too long.
    ///     Some("Really long message to filter out") ];
    /// 
    /// List&lt;Option&lt;string&gt;&gt; filtered = 
    ///     await collection
    ///         .Async()
    ///         .FilterAsync(filterCriteria)
    ///         .PipeAsync(values => values.ToList());
    /// 
    /// Assert.AreEqual(filtered[0], Some("short"));
    /// Assert.AreEqual(filtered[1], None&lt;string&gt;());
    /// Assert.AreEqual(filtered[2], None&lt;string&gt;());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of each Option.</typeparam>
    /// <param name="collection">The collection to filter.</param>
    /// <param name="predicate">The predicate filter criteria.</param>
    /// <returns>The collection with unmatched items converted to None.</returns>
    public static async Task<IEnumerable<Option<T>>> FilterAsync<T>(this Task<IEnumerable<Option<T>>> collection, Func<T, Task<bool>> predicate) where T : notnull
    {
        var output = new List<Option<T>>();

        var input = await collection;

        foreach (var option in input)
        {
            if (option.IsNone) output.Add(option);
            else if (option.IsSome && await predicate(option.Unwrap())) output.Add(option);
            else output.Add(new Option<T>());
        }

        return output;
    }

    /// <summary>
    /// Collect only values that are Some, returning their inner values.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(1), None&lt;int&gt;(), Some(2), Some(3) ];
    /// 
    /// var values = collection.Collect();
    /// Assert.AreEqual(values, [1, 2, 3]);
    /// 
    /// IEnumerable&lt;Option&lt;int&gt;&gt; emptyCollection = [];
    /// 
    /// values = emptyCollection.Collect():
    /// Assert.AreEqual(values, []);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="collection">The collection to collect values from.</param>
    /// <returns>The inner values from all of the options that were Some.</returns>
    public static IEnumerable<T> Collect<T>(this IEnumerable<Option<T>> collection) where T : notnull =>
        collection
            .Where(option => option.IsSome)
            .Select(option => option.Unwrap());

    /// <summary>
    /// Collect only values that are Some, returning their inner values.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(1), None&lt;int&gt;(), Some(2), Some(3) ];
    /// 
    /// var values = await collection.Async().CollectAsync();
    /// Assert.AreEqual(values, [1, 2, 3]);
    /// 
    /// IEnumerable&lt;Option&lt;int&gt;&gt; emptyCollection = [];
    /// 
    /// values = await emptyCollection.Async().CollectAsync():
    /// Assert.AreEqual(values, []);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="collection">The collection to collect values from.</param>
    /// <returns>The inner values from all of the options that were Some.</returns>
    public static async Task<IEnumerable<T>> CollectAsync<T>(this Task<IEnumerable<Option<T>>> collection) where T : notnull =>
        (await collection).Collect();
}