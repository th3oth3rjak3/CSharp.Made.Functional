# Examples

## Option
The Option type represents the concept that an item may or may not exist. In some ways C# already does this with the idea of the null value. However, in previous .NET and C# versions, objects could still be null even if their type signature indicated that they were a real, valid object. External packages may still have this issue, but internally you can force your code to obey the nullable rules using the Nullable directive in .NET 6 and later.
```xml title="ScratchPad.csproj"
<Nullable>enable</Nullable>
```

The following example shows how previous versions of .NET and C# would allow possible null returns. Using `FirstOrDefault` can produce a null value.


```C# title="Valid or null...?" linenums="1" hl_lines="22 32"
namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };


    // Hides its true return type which should be 'TodoItem?'
    public static TodoItem FindTodoItem(string title)
    {

        var todoItem =
            TodoItems
                .Where(todo => todo.Title.Contains(title))
                .FirstOrDefault();

        return todoItem;
    }

    public static void Main()
    {
        TodoItem todoItem = FindTodoItem("Title Which Doesn't Exist");

        // What does this print?
        Console.WriteLine(todoItem.Title);
    }
}
```
### Optional

What if we could make the return type more explicit, beyond just changing the return type to `TodoItem?` for the `FindTodoItem` method? Shown in the example below, the `Option` type helps to declare that the return type might not exist. We can use the `Optional` extension to wrap values which may be null. When a value does not exist, it is represented by `None`. If a value exists, it is represented by `Some`. 

There are ways to get the internal value of the Option and those will be described in later examples. Looking at the previous example again, let's use the Optional extension method to improve the function signature.

```C# title="Program.cs" linenums="1" hl_lines="18 22"
using Functional.Options;

namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };


    // Honest function signature which indicates optional value.
    public static Option<TodoItem> TryFindTodoItem(string title) =>
        TodoItems
            .Where(todo => todo.Title.Contains(title))
            .FirstOrDefault()
            .Optional();

    public static void Main()
    {
        Option<TodoItem> todoItem = TryFindTodoItem("Title Which Doesn't Exist");

        // Ok, but how do we print the title now?
    }
}

```
### Some and None
To create a `Some` or `None` directly, without using the `Optional` extension, the `Option` class provides helpers for this.

```C# title="Program.cs" linenums="1" hl_lines="11 16 21"
using Functional.Options;

namespace ScratchPad;

public static class Program
{
    public static void Main()
    {
        var someMessage =
            // Type is inferred to be string.
            Option.Some("Hello, world")
                .Reduce("Won't be me!");

        var someOtherMessage =
            "It's an extension method too!"
                .Some()
                .Reduce("Still not me!");

        var noneMessage =
            // Type can't be inferred, so one must be provided.
            Option.None<string>()
                .Reduce("It was None");

        // Prints "Hello, world"
        Console.WriteLine(someMessage);

        // Prints "It's an extension method too!"
        Console.WriteLine(someOtherMessage);

        // Prints "It was None"
        Console.WriteLine(noneMessage);
    }
}
```


### Map, Filter, and Reduce
In the previous example, we improved the function signature of the `TryFindTodoItem` method. Let's go a step further and find out how we can actually get the value of out an `Option` type using `Map`, `Filter`, and `Reduce`. The `Map` method performs mapping on the internal type that is wrapped by the `Option`. In the example, our internal type is a `TodoItem`. Below we use it to get the title using `Map` in the case that the `Option` is a `Some`. How do we represent a title if the `Option` is `None`? This is where `Reduce` comes in. We can supply an alternate value directly, or we can supply a function that returns an alternate value. The function approach is useful in cases where getting an alternate value might be computationally expensive. Finally, `Filter` is used to convert a `Some` to a `None` when the filter criteria evaluates to true.


```C# title="Program.cs" linenums="1" hl_lines="26-27 31-33 37 40"
using Functional.Options;

namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };

    public static Option<TodoItem> TryFindTodoItem(string title) =>
        TodoItems
            .Where(todo => todo.Title.Contains(title))
            .FirstOrDefault()
            .Optional();

    public static void Main()
    {
        string readTheDocs =
            TryFindTodoItem("Read")
                .Map(todoItem => todoItem.Title)
                .Reduce("Not Found");

        string notFound =
            TryFindTodoItem("Read")
                .Filter(todoItem => todoItem.Completed == false)
                .Map(todoItem => todoItem.Title)
                .Reduce(() => "Not Found");


        // Prints "Read the Docs" because the item exists.
        Console.WriteLine(readTheDocs);

        // Prints "Not Found" because the TodoItem is Completed, so it is filtered out.
        Console.WriteLine(notFound);
    }
}
```
### Match
Sometimes `Map` and `Reduce` may not feel like the right solution to a problem. In those cases it may feel more natural to reach for the `Match` method. `Match` expects two mapping functions to be provided to extract the value of the `Option`. See below for an alternate approach using `Match`.
```C# title="Program.cs" linenums="1" hl_lines="27-29 34-36"
using Functional.Common;
using Functional.Options;

namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };

    public static Option<TodoItem> TryFindTodoItem(string title) =>
        TodoItems
            .Where(todo => todo.Title.Contains(title))
            .FirstOrDefault()
            .Optional();

    public static void Main()
    {
        string readTheDocs =
            TryFindTodoItem("Read")
                .Match(
                    todoItem => todoItem.Title,
                    () => "Not Found");

        string notFound =
            TryFindTodoItem("Read")
                .Filter(todoItem => todoItem.Completed == false)
                .Match(
                    todoItem => todoItem.Title,
                    () => "Not Found");

        // Prints "Read the Docs" because the item exists.
        Console.WriteLine(readTheDocs);

        // Prints "Not Found" because the TodoItem is Completed, so it is filtered out.
        Console.WriteLine(notFound);
    }
}
```
### Bind
Since the `Map` function performs an action on an `Option` only when it is `Some`, the return type of `Map` is still `Option`. This means that inside the mapping operation, we usually want to do transformations that don't result in another `Option` type, otherwise we would end up with `Option<Option<T>>`. This is the reason for the `Bind` function. We can use `Bind` instead of `Map`.
```C# title="Program.cs" linenums="1" hl_lines="9 13 24"
using Functional.Options;

namespace ScratchPad;

public static class Program
{

    // This could be some database call which may result in None
    public static Option<string> TryFindString(string input) =>
        Option.Some(input);

    // Trivial example, but shows how mapping to an option requires Bind.
    public static Option<string> NoEmptyStringsAllowed(string input) =>
        string.IsNullOrWhiteSpace(input) switch
        {
            true => Option.None<string>(),
            false => input.Some(),
        };

    public static void Main()
    {
        var toPrint =
            TryFindString("Hello, World!")
                .Bind(NoEmptyStringsAllowed)
                .Reduce("Not Found");

        // Prints "Hello, World!"
        Console.WriteLine(toPrint);
    }
}
```


### Effect
If we just want to perform some `Action` which is somewhat of a side-effect, we can use the `Effect` method. Using the previous example, we can significantly simplify the code if we just want to do a single `Console.WriteLine`. We also don't have to print anything if the `TodoItem` is `None`.

```C# title="Program.cs" linenums="1"
using Functional.Options;

namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };

    public static Option<TodoItem> TryFindTodoItem(string title) =>
        TodoItems
            .Where(todo => todo.Title.Contains(title))
            .FirstOrDefault()
            .Optional();

    public static void Main() =>
        TryFindTodoItem("Read")
            .Effect(
                todoItem => Console.WriteLine(todoItem.Title),
                () => { /* We don't have to print anything actually... */ });
}
```

### Async Options

Asynchronous support is also provided in this library. The `Optional` extension also works on `Task<T>` where `T` is some type. This means that `Task<T>` or `Task<T?>` becomes `Task<Option<T>>`.

Async Option Methods:

- `FilterAsync`
- `MapAsync`
- `ReduceAsync`
- `MatchAsync`
- `BindAsync`
- `EffectAsync`

```C# title="Program.cs" linenums="1" hl_lines="22-23 29-30 34-36"
using Functional.Common;
using Functional.Options;

namespace ScratchPad;

public record TodoItem(string Title, bool Completed);

public static class Program
{
    public static List<TodoItem> TodoItems =>
        new()
        {
            new("Use CSharp.Made.Functional", false),
            new("Read the Docs", true)
        };

    // AsAsync is an extension method to wrap an item with a Task.
    public static async Task<Option<TodoItem>> TryFindTodoItem(string title) =>
        await TodoItems
            .Where(todo => todo.Title.Contains(title))
            .FirstOrDefault()
            .AsAsync()
            .Optional();

    public static async Task Main()
    {
        string readTheDocs =
            await TryFindTodoItem("Read")
                .MapAsync(todoItem => todoItem.Title)
                .ReduceAsync("Not Found");

        string notFound =
            await TryFindTodoItem("Read")
                .FilterAsync(todoItem => todoItem.Completed == false)
                .MapAsync(todoItem => todoItem.Title)
                .ReduceAsync("Not Found");


        // Prints "Read the Docs" because the item exists.
        Console.WriteLine(readTheDocs);

        // Prints "Not Found" because the TodoItem is Completed, so it is filtered out.
        Console.WriteLine(notFound);
    }
}
```

## Union
The Discriminated Union represents a type which may have multiple sub-types called variants. This is a very powerful concept which can be used to model many different real-world relationships in programming. Here is an example of what this looks like in F#. Using the built-in `match` expression in F#, we have a compile-time exhaustive match expression.

```F# title="Program.fs" linenums="1"
type Animal =
    | Cat
    | Dog
    | Bird

let GetMysteryAnimal () : Animal = Bird

let MakeAnimalNoises () : string =
    match GetMysteryAnimal() with
    | Cat -> "Meow"
    | Dog -> "Ruff"
    | Bird -> "Tweet"
```
In C#, this concept is often modeled using inheritance. This also means that we can't predict every possible type which could inherit from the original `Animal` class. So, the switch expression is as close to the F# `match` as we can get. Unfortunately for us, we always have one last annoying discard case. At best, we have a case which is never used, but is required for the code to compile. At worst, we probably have undetected logic bugs. For instance, when another variant is added as an `Animal` type, which should have a real animal sound, it will return "What Goes Here" instead of a real value. We are sadly never alerted by the compiler that we haven't handled the new variant. Problematic, indeed.

```C# title="Program.cs" linenums="1" hl_lines="3-6 19"
namespace ScratchPad;

public class Animal { }
public class Bird : Animal { }
public class Dog : Animal { }
public class Cat : Animal { }

public static class Program
{
    public static Animal GetMysteryAnimal() =>
        new Bird();

    public static string MakeAnimalNoises() =>
        GetMysteryAnimal() switch
        {
            Bird => "Tweet",
            Dog => "Ruff",
            Cat => "Meow",
            _ => "What Goes Here?",
        };

    public static void Main() { }
}
```

### Create your own Union

What if we could have compile-time type checking that ensured we handled every variant? It is possible using the `Union` type. In fact, `Option` and `Result` use the `Union` type underneath. In order to ensure strong compile-time variant checking, there are different names for each `Union` type to handle different numbers of variants. For example, `Option` uses the regular `Union` type because it has two variants, `Some` and `None`. In our animal example, however, we have 3 variants that we care about. So in this case, we need to use `Union3`. The following list shows the maximum number of variants that are currently supported by this library. I would posit that if more variants are needed, it's possible that another data modeling strategy may be a better fit. Remember, every variant must be handled, so for `Union9` this means that every match expression must have 9 different functions in order to handle every possible case.

Currently supported Unions:

- `Union`
- `Union3`
- `Union4`
- `Union5`
- `Union6`
- `Union7`
- `Union8`
- `Union9`

This implementation is a lot more verbose than the F# version, which is why C# needs built-in support for discriminated unions. However, it does accomplish the same goal. It is recommended that you seal any `Union` classes that you create in order to make sure that other classes cannot inherit from it. There must be a public constructor for each variant and no other constructors. It's also recommended that you implement the `Match` method as shown below to access the internal `Union` contents and use the underlying `Union.Match` method. It is also recommended to create static factory methods named after each variant. This will simplify the way that `Animal` instances are created.

```C# title="Program.cs" linenums="1" hl_lines="8-10 12-14 16-18 41-44"
using Functional.Unions;

namespace ScratchPad;

public sealed class Animal
{
    private Union3<Bird, Dog, Cat> contents;
    public Animal(Bird bird) => contents = new(bird);
    public Animal(Cat cat) => contents = new(cat);
    public Animal(Dog dog) => contents = new(dog);

    public T Match<T>(Func<Bird, T> whenBird, Func<Dog, T> whenDog, Func<Cat, T> whenCat) =>
        contents
            .Match(whenBird, whenDog, whenCat);

    public static Animal Cat() => new(new Cat());
    public static Animal Dog() => new(new Dog());
    public static Animal Bird() => new(new Bird());
}
public record Bird()
{
    public string Tweet => "Tweet";
}
public record Cat()
{
    public string Meow => "Meow";
}
public record Dog()
{
    public string Bark => "Ruff";
}


public static class Program
{
    public static Animal GetMysteryAnimal() =>
        Animal.Bird();

    public static string MakeAnimalNoises() =>
        GetMysteryAnimal()
            .Match(
                bird => bird.Tweet,
                dog => dog.Bark,
                cat => cat.Meow);

    public static void Main()
    {
        // Prints "Tweet"
        Console.WriteLine(MakeAnimalNoises());
    }
}
```

### Errors instead of Exceptions

C# makes heavy use of exceptions, but not all behavior is truly exceptional. For instance, if an item can't be found in a database, should we throw a `NotFoundException` and cause the server to crash if it's not handled? Instead, we can use the `Union` type to create an error type which can be used to model errors that could happen during processing. Here is an example of an error which could happen when performing database operations with an `Animal` object. In a web application, instead of returning a string, we could match the type of error and return an `HttpResponse` related to that type of error as an example.

```C# title="Program.cs" linenums="1" hl_lines="22 25 28"
using Functional.Unions;

namespace ScratchPad;

public sealed class AnimalError
{
    private Union3<NotFound, Invalid, Unhandled> contents;
    public AnimalError(NotFound notFound) => contents = new(notFound);
    public AnimalError(Invalid invalid) => contents = new(invalid);
    public AnimalError(Unhandled unhandled) => contents = new(unhandled);

    public T Match<T>(Func<NotFound, T> notFound, Func<Invalid, T> invalid, Func<Unhandled, T> unhandled) =>
        contents
            .Match(notFound, invalid, unhandled);

    public static AnimalError NotFound() => new(new NotFound());
    public static AnimalError Invalid() => new(new Invalid());
    public static AnimalError Unhandled() => new(new Unhandled());
}

// Like a 404
public record NotFound();

// Like a 400
public record Invalid();

// Like a 500
public record Unhandled();


public static class Program
{
    public static AnimalError DatabaseOperationWhichErrors() =>
        AnimalError.NotFound();

    public static string CreateNewAnimal() =>
        DatabaseOperationWhichErrors()
            .Match(
                notFound => "Animal was not found",
                invalid => "The animal was invalid",
                unhandled => "Something unexpected happened");

    public static void Main()
    {
        // Prints "Animal was not found"
        Console.WriteLine(CreateNewAnimal());
    }
}
```

## Result
In the previous example, the database operation only ever returned an error. But in real life, database operations would produce a good result sometimes, and an error result other times. We could roughly categorize these as successes and failures. In trying to keep with the same syntax as F#, this library calls successes `Ok` and failures `Error`. This is what a `Result` looks like in F#.

```F# title="Program.fs" linenums="1" hl_lines="9-10"
open System

type CustomError = { message: string }

let NoEmptyStrings (input: string) : Result<string, CustomError> =
    input
    |> String.IsNullOrWhiteSpace
    |> function
        | true -> Error { message = "Empty strings are not allowed." }
        | false -> Ok input
```

If you're not accustomed to F#, try not to focus too much on the syntax, just recognize that we can return either an `Ok` or an `Error` from the function and that's perfectly fine. In the example you will notice a few operators called `Pipe` which look like this `|>`. We will cover how CSharp.Made.Functional includes a similar feature in later discussions.

Using the previous database example, let's now take a look at how we can use the `Result` type to handle cases where the return type might be `Ok` and sometimes it might be `Error`.
```C# title="Program.cs" linenums="1" hl_lines="23 26"
using Functional.Common;
using Functional.Options;
using Functional.Results;

namespace ScratchPad;

public record NotFoundError(string Message);

public static class Program
{

    // Let's use random to decide if finding the string succeeded or not.
    // Imagine this is a database call or some other error prone operation.
    public static Option<string> TryFindString(string input) =>
        (new Random().Next(2) == 1) switch
        {
            true => input.Some(),
            false => Option.None<string>()
        };

    public static Result<string, NotFoundError> StringFindingService(string input) =>
        TryFindString(input)
            .Map(Result.Ok<string, NotFoundError>)
            .Reduce(() =>
                new NotFoundError($"'{input}' was not found.")
                    .Pipe(Result.Error<string, NotFoundError>));
    public static void Main()
    {
        for (var i = 0; i < 10; i++)
        {
            var result =
                StringFindingService("Hello, Results!")
                    .Reduce(error => error.Message);

            Console.WriteLine(result);
        }
    }
}
```

The example shows how we can use `Result.Ok` and `Result.Error` to return a `Result` type. In C#, we need to be a little bit more explicit about the types to make the compiler happy, so this is why the example is using `Result.Ok<string, NotFoundError>` where in F# that wouldn't be needed. Astute observers would also notice that there are similar methods on `Result` as there are on `Option`, which is quite on purpose. The aim of this library is to be consistent and easy to use. It will feel much more natural to reach for `Match`, `Map`, and `Reduce` when they're used for multiple types throughout the library.

### Ok and Error
To create a `Result` that is either `Ok` or `Error`, simply use the factory methods on the `Result` class. As shown below, the type signatures can get quite long sometimes. In most cases I prefer to use `var` instead of the specific type, but for clarity I have shown them here. In the example below, I have created a simple `NotFoundError` type, but using what was discussed in the `Union` section, this could also easily be a `Union` type with multiple error variants. This is where the `Result` type really shines.
```C# title="Program.cs" linenums="1"
using Functional.Results;

namespace ScratchPad;

public record NotFoundError(string Message);

public static class Program
{
    public static void Main()
    {
        Result<string, NotFoundError> ok = Result.Ok<string, NotFoundError>("It's okay");
        Result<string, NotFoundError> error = 
            Result.Error<string, NotFoundError>(new NotFoundError("An error message"));
    }
}
```


## Common Extensions

TODO

## Exception Handling

TODO