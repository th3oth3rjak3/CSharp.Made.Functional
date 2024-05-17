namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Create a new list of items.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Create a new list by providing items during the development process.
    /// List&lt;string&gt; list = Cons("item one", "item two");
    /// 
    /// // Create a new empty list.
    /// List&lt;string&gt; empty = Cons&lt;string&gt;();
    /// 
    /// // Create a list from existing items
    /// string[] items = new string[] { "hello", "world!" };
    /// List&lt;string&gt; withItems = Cons(items);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <param name="items">Items to add to the list.</param>
    /// <returns>A new list from the input values.</returns>
    public static IEnumerable<T> Cons<T>(params T[] items) =>
        items;

    /// <summary>
    /// Append a collection of items to the original collection.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;string&gt; collection = Cons("1", "2");
    /// IEnumerable&lt;string&gt; items = Cons("3", "4");
    /// IEnumerable&lt;string&gt; updated = collection.Append(items);
    /// 
    /// Assert.AreEqual(collection.Count(), 2);
    /// Assert.AreEqual(updated.Count(), 4);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="collection">The original collection.</param>
    /// <param name="items">The new items to add.</param>
    /// <returns>A new collection with both sets of items.</returns>
    public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, IEnumerable<T> items) =>
        collection.Concat(items);

    /// <summary>
    /// Append a collection of items to the original collection.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;string&gt; collection = Cons("1", "2");
    /// IEnumerable&lt;string&gt; updated = collection.Append("3", "4");
    /// 
    /// Assert.AreEqual(collection.Count(), 2);
    /// Assert.AreEqual(updated.Count(), 4);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="collection">The original collection.</param>
    /// <param name="items">The new items to add.</param>
    /// <returns>A new collection with both sets of items.</returns>
    public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, params T[] items) =>
        collection.Concat(items);

}
