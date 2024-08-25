namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// When an Option is Some, map the existing value to a new type with a provided function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = await some.Async().MapAsync(value => value.ToString());
    /// Assert.IsTrue(mapped.IsSome);
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = await none.Async().MapAsync(value => value.ToString());
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="optional">The option to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> optional,
        Func<T, TResult> mapper)
        where T : notnull
        where TResult : notnull
    {
        var result = await optional;
        return result.Map(mapper);
    }

    /// <summary>
    /// When an Option is Some, map the existing value to a new type with a provided function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = await some.Async().MapAsync(value => value.ToString().Async());
    /// Assert.IsTrue(mapped.IsSome);
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = await none.Async().MapAsync(value => value.ToString().Async());
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="optional">The option to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> optional,
        Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull
    {
        var result = await optional;

        if (result.IsNone) return None<TResult>();

        return (await mapper(result.Unwrap()))
            .Optional();
    }

    /// <summary>
    /// When an Option is Some, map the existing value to a new type with a provided function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = await some.Async().MapAsync(() => "Some".Async());
    /// Assert.IsTrue(mapped.IsSome);
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = await none.Async().MapAsync(() => "Some".Async());
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="optional">The option to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> optional,
        Func<Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull
    {
        var theOption = await optional;

        if (theOption.IsSome) return new Option<TResult>(await mapper());
        return None<TResult>();
    }

    /// <summary>
    /// When an Option is Some, map the existing value to a new type with a provided function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = await some.Async().MapAsync(() => "Some");
    /// Assert.IsTrue(mapped.IsSome);
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = await none.Async().MapAsync(() => "Some");
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the original option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="optional">The option to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<TResult>> MapAsync<T, TResult>(
        this Task<Option<T>> optional,
        Func<TResult> mapper)
        where T : notnull
        where TResult : notnull
    {
        var theOption = await optional;
        if (theOption.IsSome) return new Option<TResult>(mapper());

        return None<TResult>();
    }

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = collection.Map(value => value.ToString());
    ///         
    /// Assert.AreEqual(mapped, [ Some("123"), None&lt;string&gt;(), Some("456") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static IEnumerable<Option<TResult>> Map<T, TResult>(this IEnumerable<Option<T>> collection, Func<T, TResult> mapper) where T : notnull where TResult : notnull =>
        collection.Select(option => option.Map(mapper));

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = collection.Map(() => "Some");
    ///         
    /// Assert.AreEqual(mapped, [ Some("Some"), None&lt;string&gt;(), Some("Some") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static IEnumerable<Option<TResult>> Map<T, TResult>(this IEnumerable<Option<T>> collection, Func<TResult> mapper) where T : notnull where TResult : notnull =>
        collection.Select(option => option.Map(mapper));

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = await collection.Async().MapAsync(value => value.ToString());
    ///         
    /// Assert.AreEqual(mapped, [ Some("123"), None&lt;string&gt;(), Some("456") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static async Task<IEnumerable<Option<TResult>>> MapAsync<T, TResult>(this Task<IEnumerable<Option<T>>> collection, Func<T, TResult> mapper) where T : notnull where TResult : notnull =>
        (await collection).Map(mapper);

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = await collection.Async().MapAsync(() => "Some");
    ///         
    /// Assert.AreEqual(mapped, [ Some("Some"), None&lt;string&gt;(), Some("Some") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static async Task<IEnumerable<Option<TResult>>> MapAsync<T, TResult>(
        this Task<IEnumerable<Option<T>>> collection,
        Func<TResult> mapper)
        where T : notnull
        where TResult : notnull =>
            (await collection).Map(mapper);

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Task&lt;string&gt; toStringAsync(int input) => Task.FromResult(input.ToString());
    /// 
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = await collection.Async().MapAsync(toStringAsync);
    ///         
    /// Assert.AreEqual(mapped, [ Some("123"), None&lt;string&gt;(), Some("456") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static async Task<IEnumerable<Option<TResult>>> MapAsync<T, TResult>(
        this Task<IEnumerable<Option<T>>> collection,
        Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull
    {
        var output = new List<Option<TResult>>();

        var theCollection = await collection;

        foreach (var option in theCollection)
        {
            if (option.IsSome)
            {
                var mapped = await mapper(option.Unwrap());
                output.Add(new Option<TResult>(mapped));
            }

            if (option.IsNone) output.Add(new Option<TResult>());
        }

        return output;
    }

    /// <summary>
    /// Map a collection of options using a mapping function when an option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Task&lt;string&gt; getStringAsync() => Task.FromResult("Some");
    /// 
    /// IEnumerable&lt;Option&lt;int&gt;&gt; collection = [ Some(123), None&lt;int&gt;(), Some(456) ];
    /// 
    /// IEnumerable&lt;Option&lt;string&gt;&gt; mapped = await collection.Async().MapAsync(getStringAsync);
    ///         
    /// Assert.AreEqual(mapped, [ Some("Some"), None&lt;string&gt;(), Some("Some") ]);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="collection">The collection of options to map.</param>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>The mapped collection.</returns>
    public static async Task<IEnumerable<Option<TResult>>> MapAsync<T, TResult>(
        this Task<IEnumerable<Option<T>>> collection,
        Func<Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull
    {
        var output = new List<Option<TResult>>();

        var theCollection = await collection;

        foreach (var option in theCollection)
        {
            if (option.IsSome)
            {
                var mapped = await mapper();
                output.Add(new Option<TResult>(mapped));
            }

            if (option.IsNone) output.Add(new Option<TResult>());
        }

        return output;
    }
}