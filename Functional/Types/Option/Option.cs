namespace Functional;

/// <summary>
/// A wrapper class which contains either Some value or no value (None).
/// </summary>
/// <typeparam name="T">The inner type of the Option.</typeparam>
public sealed record Option<T>
{

    /// <summary>
    /// Represents the internal state of the Option.
    /// </summary>
    private enum State
    {
        Some,
        None
    }

    /// <summary>
    /// The internal contents of the Option.
    /// </summary>
    private readonly T? contents;

    /// <summary>
    /// The current state of the Option. None by default.
    /// </summary>
    private readonly State state = State.None;

    /// <summary>
    /// Create a new option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will become Some value.
    /// Option&lt;string&gt; option = new("something");
    /// 
    /// // These will become None
    /// string? uninitialized;
    /// option = new(uninitialized);
    /// option = new(null as string);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="input">Input which may be null.</param>
    public Option(T? input)
    {
        if (input is null) return;
        contents = input;
        state = State.Some;
    }

    /// <summary>
    /// Create a new option that is a None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will be a None by default.
    /// Option&lt;string&gt; option = new();
    /// </code>
    /// </example>
    /// </summary>
    public Option() { }

    /// <summary>
    /// Implicitly create a new Option type from a returned value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; DoWork(int input)
    /// {
    ///     // We can directly return a string and it will be converted to an Option.
    ///     if (input &lt; 20) return input.ToString();
    ///     return None&lt;string&gt;();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="input">The input to convert to an Option.</param>
    public static implicit operator Option<T>(T? input) =>
        new(input);

    /// <summary>
    /// Determine if an Option is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; optional = new("value");
    /// if (optional.IsSome)
    /// {
    ///     // Do something here when Some.
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public bool IsSome => state == State.Some;

    /// <summary>
    /// Determine if an Option is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; optional = new();
    /// if (optional.IsNone)
    /// {
    ///     // Do something here when None.
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public bool IsNone => state == State.None;

    /// <summary>
    /// Unwrap is used to get the inner value of an Option when the Option type
    /// contains some value. 
    /// <br/>If an option is None, it will throw an InvalidOperationException.
    /// <br/>For more information, see: <see cref="IsSome"/> or <see cref="IsNone"/>.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will be fine
    /// new Option&lt;string&gt;("hello, world!").Unwrap();
    /// 
    /// // This will throw an InvalidOperationException
    /// new Option&lt;string&gt;(null as string).Unwrap();
    /// 
    /// // To use this safely, perform the following steps
    /// Option&lt;string&gt; optional = new Option&lt;string&gt;(null as string); // This will be a None.
    /// 
    /// if (optional.IsSome)
    /// {
    ///     var contents = optional.Unwrap();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the Option.</returns>
    /// <exception cref="InvalidOperationException">Thrown when unwrapping a None.</exception>
    public T Unwrap()
    {
        if (IsSome && contents is not null) return contents;
        throw new InvalidOperationException("Tried to unwrap a None! Be sure to check with IsSome before unwrapping.");
    }

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = new Option&lt;string&gt;("some value").Match(value => value, () => "none");
    /// Assert.AreEqual(someValue, "some value");
    /// 
    /// string noneValue = new Option&lt;string&gt;().Match(value => value, () => "none");
    /// Assert.AreEqual(noneValue, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSome, Func<TResult> whenNone) =>
        IsSome
            ? whenSome(Unwrap())
            : whenNone();

    /// <summary>
    /// Match the option to either Some or None and provide functions to handle each case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string someValue = new Option&lt;string&gt;("some value").Match(() => "some", () => "none");
    /// Assert.AreEqual(someValue, "some");
    /// 
    /// string noneValue = new Option&lt;string&gt;().Match(() => "some", () => "none");
    /// Assert.AreEqual(noneValue, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSome">The function to execute when some.</param>
    /// <param name="whenNone">The function to execute when none.</param>
    /// <returns>The result of the function performed on Some or None.</returns>
    public TResult Match<TResult>(Func<TResult> whenSome, Func<TResult> whenNone) =>
        IsSome
            ? whenSome()
            : whenNone();

    /// <summary>
    /// When an Option is Some, map the existing value to a new type with a provided function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = some.Map(value => value.ToString());
    /// Assert.IsTrue(mapped.IsSome);
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = none.Map(value => value.ToString());
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public Option<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull =>
        IsSome
            ? new Option<TResult>(mapper(Unwrap()))
            : new Option<TResult>();

    /// <summary>
    /// When an Option is Some, perform a mapping function which generates a new value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;int&gt; some = Some(123);
    /// // Since the value is some, the mapping will be performed.
    /// Option&lt;string&gt; mapped = some.Map(() => "replacement value");
    /// Assert.IsTrue(mapped.IsSome);
    /// Assert.AreEqual(mapped.Unwrap(), "replacement value");
    /// 
    /// Option&lt;int&gt; none = None&lt;int&gt;();
    /// // Since the value is none, the mapping will not be performed, but instead a new None of string will be returned.
    /// mapped = none.Map(() => "replacement value");
    /// Assert.IsTrue(mapped.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The new type.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of a Some.</param>
    /// <returns>A new option.</returns>
    public Option<TResult> Map<TResult>(Func<TResult> mapper) where TResult : notnull =>
        IsSome
            ? new Option<TResult>(mapper())
            : new Option<TResult>();

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
    /// var filtered = some.Filter(filterCriteria);
    /// Assert.IsTrue(filtered.IsSome);
    /// 
    /// some = Some("a really long message that will get filtered out");
    /// 
    /// // Filtered out because the Length of the input was greater than 10 characters.
    /// filtered = some.Filter(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// 
    /// Option&lt;string&gt; none = None&lt;string&gt;();
    /// 
    /// // Input was None, so it's still a None.
    /// filtered = none.Filter(filterCriteria);
    /// Assert.IsTrue(filtered.IsNone);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="predicate">A predicate function to check if the contents of a Some match another value.</param>
    /// <returns>A new option.</returns>
    public Option<T> Filter(Func<T, bool> predicate)
    {
        // None is automatically filtered out, so return it directly.
        if (IsNone) return this;

        return predicate(Unwrap())
            ? this              // Predicate matched, keep the value
            : new Option<T>();  // Not matched, create new None.
    }

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise, return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Option&lt;string&gt; maybeString(int input) =>
    ///     input &lt; 10
    ///     ? Some(input.ToString())
    ///     : None&lt;string&gt;();
    /// 
    /// string contents = maybeString(5).Reduce("none");
    ///         
    /// Assert.AreEqual(contents, "5");
    /// 
    /// contents = maybeString(42).Reduce("none");
    ///         
    /// Assert.AreEqual(contents, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public T Reduce(T alternate) =>
        Match(
            some => some,
            () => alternate);

    /// <summary>
    /// Extract the contents of an Option when Some. Otherwise, return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Option&lt;string&gt; maybeString(int input) =>
    ///     input &lt; 10
    ///     ? Some(input.ToString())
    ///     : None&lt;string&gt;();
    /// 
    /// string contents = maybeString(5).Reduce(() => "none");
    ///         
    /// Assert.AreEqual(contents, "5");
    /// 
    /// contents = maybeString(42).Reduce(() => "none");
    ///         
    /// Assert.AreEqual(contents, "none");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public T Reduce(Func<T> alternate) =>
        Match(
            some => some,
            alternate);

    /// <summary>
    /// Perform a side effect on an option type and consume the option.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// // This will print "hello, world!"
    /// Unit unit = 
    ///     new Option&lt;string&gt;("hello, world!")
    ///         .Effect(
    ///             msg => Console.WriteLine(msg),
    ///             () => Console.WriteLine("The value was None"));
    ///         
    /// Assert.IsInstanceOfType(unit, typeof(Unit));
    /// 
    /// // This will print "The value was None"
    /// unit = 
    ///     new Option&lt;string&gt;()
    ///         .Effect(
    ///             msg => Console.WriteLine(msg),
    ///             () => Console.WriteLine("The value was None"));
    ///             
    /// Assert.IsInstanceOfType(unit, typeof(Unit));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action<T> doWhenSome, Action doWhenNone)
    {
        if (IsSome) doWhenSome(Unwrap());
        if (IsNone) doWhenNone();

        return new Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// // This will print "The value was Some"
    /// Unit unit = 
    ///     new Option&lt;string&gt;("hello, world!")
    ///         .Effect(
    ///             () => Console.WriteLine("The value was Some"),
    ///             () => Console.WriteLine("The value was None"));
    ///         
    /// Assert.IsInstanceOfType(unit, typeof(Unit));
    /// 
    /// // This will print "The value was None"
    /// unit = 
    ///     new Option&lt;string&gt;()
    ///         .Effect(
    ///             () => Console.WriteLine("The value was Some"),
    ///             () => Console.WriteLine("The value was None"));
    ///             
    /// Assert.IsInstanceOfType(unit, typeof(Unit));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit Effect(Action doWhenSome, Action doWhenNone)
    {
        if (IsSome) doWhenSome();
        if (IsNone) doWhenNone();
        return new Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will print "hello, world!"
    /// new Option&lt;string&gt;("hello, world!")
    ///     .EffectSome(msg => Console.WriteLine(msg));
    ///     
    /// // This won't print anything since the value is None.
    /// new Option&lt;string&gt;()
    ///     .EffectSome(msg => Console.WriteLine(msg));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public Unit EffectSome(params Action<T>[] doWhenSome)
    {
        if (IsNone) return Unit.Default;
        var value = Unwrap();
        doWhenSome.ToList().ForEach(action => action(value));
        return new Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will print "The value was Some"
    /// new Option&lt;string&gt;("hello, world!")
    ///     .EffectSome(() => Console.WriteLine("The value was Some"));
    ///     
    /// // This won't print anything since the value is None.
    /// new Option&lt;string&gt;()
    ///     .EffectSome(() => Console.WriteLine("The value was Some"));
    ///     
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public Unit EffectSome(params Action[] doWhenSome)
    {
        if (IsSome) doWhenSome.ToList().ForEach(action => action());
        return new Unit();
    }

    /// <summary>
    /// Perform a side effect on an option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This will print "The value was None"
    /// new Option&lt;string&gt;()
    ///     .EffectNone(() => Console.WriteLine("The value was None"));
    ///     
    /// // This will not print since the value was Some
    /// new Option&lt;string&gt;("hello, world!")
    ///     .EffectNone(() => Console.WriteLine("The value was None"));
    ///     
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public Unit EffectNone(params Action[] doWhenNone)
    {
        if (IsNone) doWhenNone.ToList().ForEach(action => action());
        return new Unit();
    }

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // value is 123 since the input was Some.
    /// int value =
    ///     Some(123)
    ///         // Tap performs side effects, then returns the value that was input.
    ///         // In this case, that means it returns Some(123) as an Option&lt;int&gt;
    ///         .Tap(
    ///             // Performed when the value is Some.
    ///             value => Console.WriteLine($"The value was: {value}"),
    ///             // Performed when the value is None.
    ///             () => Console.WriteLine("The value was: None"))
    ///         .Reduce(0);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action<T> whenSome, Action whenNone)
    {
        if (IsSome) whenSome(Unwrap());
        if (IsNone) whenNone();

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform different actions when the value is some or none.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // value is 123 since the input was Some.
    /// int value =
    ///     Some(123)
    ///         // Tap performs side effects, then returns the value that was input.
    ///         // In this case, that means it returns Some(123) as an Option&lt;int&gt;
    ///         .Tap(
    ///             // Performed when the value is Some.
    ///             () => Console.WriteLine("The value was: Some"),
    ///             // Performed when the value is None.
    ///             () => Console.WriteLine("The value was: None"))
    ///         .Reduce(0);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> Tap(Action whenSome, Action whenNone)
    {
        if (IsSome) whenSome();
        if (IsNone) whenNone();

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int temp;
    /// // Tap returns the input value.
    /// Option&lt;int&gt; some =
    ///     Some(123)
    ///         // TapSome can take multiple inputs
    ///         .TapSome(
    ///             value => Console.WriteLine(value),
    ///             value => temp = value);
    ///
    /// None&lt;int&gt;()
    ///     // Nothing happens here since the input is None.
    ///     .TapSome(
    ///         value => Console.WriteLine(value),
    ///         value => temp = value);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(params Action<T>[] whenSome)
    {
        if (IsNone) return this;
        var unwrapped = Unwrap();
        whenSome.ToList().ForEach(action => action(unwrapped));
        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is Some.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Tap returns the input value.
    /// Option&lt;int&gt; some =
    ///     Some(123)
    ///         // TapSome can take multiple inputs
    ///         .TapSome(
    ///             () => WriteCode(),
    ///             () => MakeMoney());
    ///
    /// None&lt;int&gt;()
    ///     // Nothing happens here since the input is None.
    ///     .TapSome(
    ///         () => WriteCode(),
    ///         () => MakeMoney());
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="whenSome">Perform this action when the value is Some.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapSome(params Action[] whenSome)
    {
        if (IsSome)
        {
            whenSome.ToList().ForEach(action => action());
        }

        return this;
    }

    /// <summary>
    /// Tap into the contents of the Option and perform an action when the value is None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // Tap returns the input value.
    /// None&lt;int&gt;()
    ///     // TapNone can take multiple inputs.
    ///     .TapNone(
    ///         () => WriteCode(),
    ///         () => MakeMoney());
    ///
    /// Option&lt;int&gt; some =
    ///     Some(123)
    ///         // This doesn't do anything since the input is Some.
    ///         .TapNone(
    ///             () => WriteCode(),
    ///             () => MakeMoney());
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="whenNone">Perform this action when the value is None.</param>
    /// <returns>The input value.</returns>
    public Option<T> TapNone(params Action[] whenNone)
    {
        if (IsNone)
        {
            whenNone.ToList().ForEach(action => action());
        }

        return this;
    }
}