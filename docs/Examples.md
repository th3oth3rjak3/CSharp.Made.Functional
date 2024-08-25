# Examples

## Option

The Option type represents the concept that an item may or may not exist. In some ways C# already does this with the idea of the null value. However, in previous .NET and C# versions, objects could still be null even if their type signature indicated that they were a real, valid object. External packages may still have this issue, but internally you can force your code to obey the nullable rules using the Nullable directive in .NET 6 and later.

```xml title="ScratchPad.csproj"
<Nullable>enable</Nullable>
```

The following example shows how previous versions of .NET and C# would allow possible null returns. Using `FirstOrDefault` can produce a null value.

```C# title="Valid or null...?" linenums="1" hl_lines="23 33"
using System;
using System.Collections.Generic;
using System.Linq;

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

```C# title="Program.cs" linenums="1" hl_lines="21 25"
using System.Collections.Generic;
using System.Linq;
using Functional;
using static Functional.Prelude;

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

To create a `Some` or `None` directly, without using the `Optional` extension, the `Prelude` class provides helpers for this.

```C# title="Program.cs" linenums="1" hl_lines="13 18"
using System;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public static class Program
{
    public static void Main()
    {
        var someMessage =
            // Type is inferred to be string.
            Some("Hello, world")
                .Reduce("Won't be me!");

        var noneMessage =
            // Type can't be inferred, so one must be provided.
            None<string>()
                .Reduce("It was None");

        // Prints "Hello, world"
        Console.WriteLine(someMessage);

        // Prints "It was None"
        Console.WriteLine(noneMessage);
    }
}
```

### Map, Filter, and Reduce

In the example showing the `Optional` extension, we improved the function 
signature of the `TryFindTodoItem` method. Let's go a step further and find 
out how we can actually get the value of out an `Option` type using 
`Map`, `Filter`, and `Reduce`. The `Map` method performs mapping on the 
internal type that is wrapped by the `Option`. In the example, our internal 
type is a `TodoItem`. Below we use it to get the title using `Map` in the case 
that the `Option` is a `Some`. How do we represent a title if the `Option` is `None`? 
This is where `Reduce` comes in. We can supply an alternate value directly, 
or we can supply a function that returns an alternate value. 
The function approach is useful in cases where getting an alternate value
might be computationally expensive. Finally, `Filter` is used to convert a 
`Some` to a `None` when the filter criteria evaluates to true.

```C# title="Program.cs" linenums="1" hl_lines="30-31 35-37 41 42"
using System;
using System.Collections.Generic;
using System.Linq;
using Functional;
using static Functional.Prelude;

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

### Option Match

Sometimes `Map` and `Reduce` may not feel like the right solution to a problem. 
In those cases it may feel more natural to reach for the `Match` method. 
`Match` expects two mapping functions to be provided to extract the value 
of the `Option`. See below for an alternate approach using `Match`.

```C# title="Program.cs" linenums="1" hl_lines="30-32 37-39"
using System;
using System.Collections.Generic;
using System.Linq;
using Functional;
using static Functional.Prelude;

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

### Option Bind

Since the `Map` function performs an action on an `Option` only when it 
is `Some`, the return type of `Map` is still `Option`. This means that 
inside the mapping operation, we usually want to do transformations 
that don't result in another `Option` type, otherwise we would end up 
with `Option<Option<T>>`. This is the reason for the `Bind` function. 
We can use `Bind` instead of `Map`.

```C# title="Program.cs" linenums="1" hl_lines="11 15 26"
using System;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public static class Program
{

    // This could be some database call which may result in None
    public static Option<string> TryFindString(string input) =>
        Some(input);

    // Trivial example, but shows how mapping to an option requires Bind.
    public static Option<string> NoEmptyStringsAllowed(string input) =>
        string.IsNullOrWhiteSpace(input) switch
        {
            true => None<string>(),
            false => Some(input),
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

### Option Effect

If we just want to perform some `Action` which is somewhat of a side-effect, 
we can use the `Effect` method. Using the previous example, we can 
significantly simplify the code if we just want to do a single 
`Console.WriteLine`. We also don't have to print anything if the 
`TodoItem` is `None`.

```C# title="Program.cs" linenums="1" hl_lines="28-30"
using System;
using System.Collections.Generic;
using System.Linq;
using Functional;
using static Functional.Prelude;

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
### Option EffectSome and EffectNone

If we only want to perform an effect when an `Option` is `Some` or 
when it's `None`, we can use the `EffectSome` and `EffectNone` methods 
which will consume the `Option`.


```C# title="Program.cs" linenums="1" hl_lines="13 17 21 25"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;
using System;

public static class Program
{
    public static void Main()
    {
        // This will print "value" to the console.
        Some("value")
            .EffectSome(value => Console.WriteLine(value));

        // This will do nothing since the input value was a None.
        None<string>()
            .EffectSome(value => Console.WriteLine(value));

        // This will do nothing since the input is a Some.
        Some("value")
            .EffectNone(() => Console.WriteLine("won't print"));

        // This will print "no value" since the input was None.
        None<string>()
            .EffectNone(() => Console.WriteLine("no value"));
    }
}

```

### Option Tap, TapSome and TapNone

If we want to `Tap` into the `Option` and perform some effect without 
consuming the value, we can use `Tap`, `TapSome`, and `TapNone`. 
With `Tap`, some kind of action must be provided for both 
the `Some` and `None` cases.

For `TapSome` and `TapNone`, one or more actions can be provided 
to occur when the `Option` meets that criteria. This will allow us 
to only perform actions when the value is `Some` or `None` for instance.


```C# title="Program.cs" linenums="1" hl_lines="13 20-22 26"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;
using System;

public static class Program
{
    public static void Main()
    {
        // Actions can be performed when some and none.
        Some("value")
            .Tap(some => Console.WriteLine(some), () => Console.WriteLine("none"))
            // The Option is not consumed so we can still use it afterwards.
            .Map(some => some + "!");

        // We can do multiple things when the value is Some with a TapSome.
        string? temp = null;
        Some("value")
            .TapSome(
                value => Console.WriteLine(value),
                value => temp = value);

        // Nothing happens here since the value is a None.
        None<string>()
            .TapSome(value => Console.WriteLine(value));
    }
}

```

### Option Unwrap

If we need to get the value out of an `Option` for some reason and it's 
impractical to use `Match`, `Map`, `Tap`, or `Effect`, we can `Unwrap` the 
value to get its inner contents.
It's vital to check to see if the `Option` is `Some` before doing this, 
otherwise it will throw an exception!

```C# title="Program.cs" linenums="1" hl_lines="12 15 19-22"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;
using System;

public static class Program
{
    public static void Main()
    {
        // This will unwrap fine because the value is some.
        string value = Some("value").Unwrap();

        // This will throw an exception because the value is none.
        string never = None<string>().Unwrap();

        // To do this safely, we need to always check the Option first!
        var option = None<string>();
        if (option.IsSome)
        {
            value = option.Unwrap();
        }
    }
}

```

### Async Options

Asynchronous support is also provided in this library. The `Optional` extension 
also works on `Task<T>` where `T` is some type. This means that `Task<T>` 
or `Task<T?>` becomes `Task<Option<T>>`.

Async Option Methods:

- `FilterAsync`
- `MapAsync`
- `ReduceAsync`
- `MatchAsync`
- `BindAsync`
- `EffectAsync`
- `EffectSomeAsync`
- `EffectNoneAsync`
- `TapAsync`
- `TapSomeAsync`
- `TapNoneAsync`
- `UnwrapAsync`
- `UnwrapErrorAsync`

```C# title="Program.cs" linenums="1" hl_lines="26-27 33-34 38-40"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional;
using static Functional.Prelude;

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
            .Async()
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

The Discriminated Union represents a type which may have multiple sub-types 
called variants. This is a very powerful concept which can be used to model 
many different real-world relationships in programming. Here is an example 
of what this looks like in F#. Using the built-in `match` expression in F#, 
we have a compile-time exhaustive match expression.

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

In C#, this concept is often modeled using inheritance. This also means that 
we can't predict every possible type which could inherit from the original 
`Animal` class. So, the switch expression is as close to the F# `match` as 
we can get. Unfortunately for us, we always have one last annoying discard case. 
At best, we have a case which is never used, but is required for the code to compile. 
At worst, we probably have undetected logic bugs. For instance, when 
another variant is added as an `Animal` type, which should have a real 
animal sound, it will return "What Goes Here" instead of a real value. 
We are sadly never alerted by the compiler that we haven't handled the new 
variant. Problematic, indeed.

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

What if we could have compile-time type checking that ensured we handled every 
variant? It is possible using the `Union` type. In fact, `Option` and `Result` 
use the `Union` type underneath. In order to ensure strong compile-time variant 
checking, there are different names for each `Union` type to handle different 
numbers of variants. For example, `Option` uses the regular `Union` type because 
it has two variants, `Some` and `None`. In our animal example, however, we have
3 variants that we care about. So in this case, we need to use `Union3`. The 
following list shows the maximum number of variants that are currently supported 
by this library. I would posit that if more variants are needed, it's possible 
that another data modeling strategy may be a better fit. Remember, every variant
must be handled, so for `Union9` this means that every match expression must 
have 9 different functions in order to handle every possible case.

Currently supported Unions:

- `Union`
- `Union3`
- `Union4`
- `Union5`
- `Union6`
- `Union7`
- `Union8`
- `Union9`

This implementation is a lot more verbose than the F# version, which is why C# 
needs built-in support for discriminated unions. However, it does accomplish 
the same goal. It is recommended that you seal any `Union` classes that you 
create in order to make sure that other classes cannot inherit from it. 
There must be a public constructor for each variant and no other constructors. 
It's also recommended that you implement the `Match` method as shown below 
to access the internal `Union` contents and use the underlying `Union.Match`
method. It is also recommended to create static factory methods named after 
each variant. This will simplify the way that `Animal` instances are created.

```C# title="Program.cs" linenums="1" hl_lines="9-11 13-15 17-19 42-45"
using System;
using Functional;
using static Functional.Prelude;
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

C# makes heavy use of exceptions, but not all behavior is truly exceptional. 
For instance, if an item can't be found in a database, should we throw a 
`NotFoundException` and cause the server to crash if it's not handled? 
Instead, we can use the `Union` type to create an error type which can be used 
to model errors that could happen during processing. Here is an example of 
an error which could happen when performing database operations with an 
`Animal` object. In a web application, instead of returning a string, 
we could match the type of error and return an `HttpResponse` related to 
that type of error as an example.

```C# title="Program.cs" linenums="1" hl_lines="23 26 29"
using System;
using Functional;
using static Functional.Prelude;
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

### Built-in Methods

All of the `Union` variants have `Effect` and `Match` built-in. This means that
if you create a custom `Union` type, you can use the inner `Union` built-in 
methods to expose public versions on your custom `Union` type. An example of 
this is shown in the previous `Animal` example with the `Match` method.

## Result

In the previous example, the database operation only ever returned an error. 
But in real life, database operations would produce a good result sometimes, 
and an error result other times. We could roughly categorize these as successes 
and failures. In trying to keep with the same syntax as F#, this library calls 
successes `Ok` and failures `Error`. This is what a `Result` looks like in F#.

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

If you're not accustomed to F#, try not to focus too much on the syntax, 
just recognize that we can return either an `Ok` or an `Error` from the 
function and that's perfectly fine. In the example you will notice a few 
operators called `Pipe` which look like this `|>`. We will cover how 
CSharp.Made.Functional includes a similar feature in later discussions.

Using the previous database example, let's now take a look at how we can use 
the `Result` type to handle cases where the return type might be `Ok` and 
sometimes it might be `Error`.

```C# title="Program.cs" linenums="1" hl_lines="24 27"
using System;
using Functional;
using static Functional.Prelude;


namespace ScratchPad;

public record NotFoundError(string Message);

public static class Program
{

    // Let's use random to decide if finding the string succeeded or not.
    // Imagine this is a database call or some other error prone operation.
    public static Option<string> TryFindString(string input) =>
        (new Random().Next(2) == 1) switch
        {
            true => Some(input),
            false => None<string>()
        };

    public static Result<string, NotFoundError> StringFindingService(string input) =>
        TryFindString(input)
            .Map(Ok<string, NotFoundError>)
            .Reduce(() =>
                new NotFoundError($"'{input}' was not found.")
                    .Pipe(Error<string, NotFoundError>));
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

The example shows how we can use `Ok` and `Error` to return a 
`Result` type. In C#, we need to be a little bit more explicit about the 
types to make the compiler happy, so this is why the example is using 
`Ok<string, NotFoundError>` where in F# that wouldn't be needed. Astute 
observers would also notice that there are similar methods on `Result` as 
there are on `Option`, which is quite on purpose. The aim of this library 
is to be consistent and easy to use. It will feel much more natural to reach 
for `Match`, `Map`, and `Reduce` when they're used for multiple types throughout 
the library.

### Ok and Error

To create a `Result` that is either `Ok` or `Error`, simply use the convenience 
methods on the `Prelude` class. As shown below, the type signatures can get quite 
long sometimes. In most cases I prefer to use `var` instead of the specific type, 
but for clarity I have shown them here. In the example below, I have created 
a simple `NotFoundError` type, but using what was discussed in the `Union` 
section, this could also easily be a `Union` type with multiple error variants. 
This is where the `Result` type really shines.

```C# title="Program.cs" linenums="1"
using Functional.Results;

namespace ScratchPad;

public record NotFoundError(string Message);

public static class Program
{
    public static void Main()
    {
        Result<string, NotFoundError> ok = Ok<string, NotFoundError>("It's okay");
        Result<string, NotFoundError> error = 
            Error<string, NotFoundError>(new NotFoundError("An error message"));
    }
}
```

### Map and Reduce

Just like the `Option` type, `Result` has `Map` and `Reduce` methods. `Map`
works exactly the same way, in that if the `Result` is `Ok`, then it performs 
the mapping function on the inner contents. `Reduce`, however has multiple 
overloaded methods. It can be used in a way that discards the error and returns 
an alternate value, or it can use the error to then create the alternate value.

The `Map` function can be called multiple times to perform different 
transformations therefore pipelining the values from the previous transformation 
to the next one. This can greatly improve the readability of what is happening to 
a value as it goes through the pipeline. The example below is contrived in that 
you could perform the mapping steps all in one operation, but for the sake of 
demonstration, it's been broken out into multiple `Map` operations.

`TryPayForMovies` returns a `Result<int, InsufficientFundsError>` which for our 
purposes means that sometimes the balance is `Ok` and sometimes, usually 
according to some business or domain logic, it could be an `Error`. Through 
multiple `Map` steps, we can convert the number to a string, add a `$`, then 
add some more formatting for readability through the power of composition. To 
demonstrate various cases, the example uses `Enumerable.Range` to print 
different results 10 times.

```C# title="Program.cs" linenums="1" hl_lines="12 29-32"
using System;
using System.Linq;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record InsufficientFundsError(string Message);

public static class Program
{
    public static Result<int, InsufficientFundsError> TryPayForMovies() =>
        // Let's assume that movies cost $25
        new Random().Next(100) switch
        {
            var balance when balance > 25 =>
                (balance - 25)
                    .Pipe(Ok<int, InsufficientFundsError>),
            var balance =>
                new InsufficientFundsError($"Movie Error: balance of ${balance} is too low to pay for movies.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };
    public static void Main() =>
        Enumerable
            .Range(0, 30)
            .ToList()
            .ForEach(_ =>
                TryPayForMovies()
                    .Map(balance => balance.ToString())
                    .Map(strBalance => $"${strBalance}")
                    .Map(withDollarSign => $"Your balance after paying for the movies: {withDollarSign}")
                    .Reduce(err => err.Message)
                    .Tap(Console.WriteLine)
                    .Ignore());
}
```

### Map Errors

In some cases, it will be necessary to convert one error type to another,
just like when performing a regular mapping function. In this case, reach 
for the `MapError` method.

```C# title="Program.cs" linenums="1" hl_lines="15 27 36 52"
using System;
using System.Collections.Generic;
using System.Linq;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record InsufficientFundsError(string Message);

public record ApplicationError(List<string> Errors);

public static class Program
{
    public static Result<int, InsufficientFundsError> TryPayForMovies() =>
        // Let's assume that movies cost $25
        new Random().Next(100) switch
        {
            var balance when balance > 25 =>
                (balance - 25)
                    .Pipe(Ok<int, InsufficientFundsError>),
            var balance =>
                new InsufficientFundsError($"Movie Error: balance of ${balance} is too low to pay for movies.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };

    public static void PrintErrorMessages(this Result<string, ApplicationError> result)
    {
        result
            .Effect(
                ok => { },
                err => err.Errors.ForEach(Console.WriteLine)
            );
    }

    public static ApplicationError ToApplicationError(this InsufficientFundsError error)
    {
        var errorMessage = error.Message;
        var errorList = new List<string> { errorMessage };
        return new ApplicationError(errorList);
    }

    public static void Main() =>
        Enumerable
            .Range(0, 30)
            .ToList()
            .ForEach(_ =>
                TryPayForMovies()
                    .Map(balance => balance.ToString())
                    .Map(strBalance => $"${strBalance}")
                    .Map(withDollarSign => $"Your balance after paying for the movies: {withDollarSign}")
                    .MapError(err => err.ToApplicationError())
                    .Tap(err => err.PrintErrorMessages())
                    .Ignore());
}

```

### Result Match

The previous example can be rewritten using `Match` as well. However, as previously 
noted, `Map` can be called many times, where `Match` can only be called once. 
`Match` should be thought of as a replacement for a single `Map` and `Reduce`
operation. This way, if there are many `Map` operations to perform, it is possible
to do all of them except the last one, then call `Match`.

```C# title="Program.cs" linenums="1" hl_lines="30-34"
using System;
using System.Linq;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record InsufficientFundsError(string Message);

public static class Program
{
    public static Result<int, InsufficientFundsError> TryPayForMovies() =>
        // Let's assume that movies cost $25
        new Random().Next(100) switch
        {
            var balance when balance > 25 =>
                (balance - 25)
                    .Pipe(Ok<int, InsufficientFundsError>),
            var balance =>
                new InsufficientFundsError($"Movie Error: balance of ${balance} is too low to pay for movies.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };

    public static void Main() =>
        Enumerable
            .Range(0, 30)
            .ToList()
            .ForEach(_ =>
                TryPayForMovies()
                    .Map(balance => balance.ToString())
                    .Map(strBalance => $"${strBalance}")
                    .Match(
                        withDollarSign => $"Your balance after paying for the movies: {withDollarSign}",
                        err => err.Message)
                    .Tap(Console.WriteLine)
                    .Ignore());
}
```

### Result Bind

Just like the `Bind` method for `Option`, `Result` also has a `Bind` method for 
when a mapping operation produces another `Result` type. There is a current 
limitation on this method where the binding method must also produce the same 
error type. This usually isn't a problem, but something to be aware of. Much of 
this limitation can be overcome by using Discriminated Unions for an error type. 
This way, a binding method could produce a different error perhaps, but if it's a 
variant of a discriminated union, C# will still see it as the parent discriminated
union type. In the following example, let's say that we wanted to pay for a movie 
and buy dinner. Buying dinner though, depends on whether or not there was money 
left over after paying for the movies.

```C# title="Program.cs" linenums="1" hl_lines="24 42"
using System;
using System.Linq;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record InsufficientFundsError(string Message);

public static class Program
{
    public static Result<int, InsufficientFundsError> TryPayForMovies() =>
        // Let's assume that movies cost $25
        new Random().Next(100) switch
        {
            var balance when balance > 25 =>
                (balance - 25)
                    .Pipe(Ok<int, InsufficientFundsError>),
            var balance =>
                new InsufficientFundsError($"Movie Error: balance of ${balance} is too low to pay for movies.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };

    public static Result<int, InsufficientFundsError> TryBuyDinner(int balance) =>
        // assuming dinner costs $50
        balance switch {
            var bal when bal >= 50 =>
                (balance - 50)
                    .Pipe(Ok<int, InsufficientFundsError>),
            _ => 
                new InsufficientFundsError($"Dinner Error: balance of ${balance} was too low to pay for dinner.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };


    public static void Main() =>
        Enumerable
            .Range(0, 30)
            .ToList()
            .ForEach(_ =>
                TryPayForMovies()
                    .Bind(TryBuyDinner)
                    .Map(balance => balance.ToString())
                    .Map(strBalance => $"${strBalance}")
                    .Map(withDollarSign => $"Your balance after paying for the movies: {withDollarSign}")
                    .Reduce(err => err.Message)
                    .Tap(Console.WriteLine)
                    .Ignore());
}
```

### Result Effect

Just like `Option` and `Union`, `Result` also has an `Effect` method that 
allows us to perform some `Action` which returns `void` as a side-effect on 
our `Result` type. `Effect` is similar to `Match`, but instead of returning 
value, both arms return `void`. We can use `Effect` in place of the `Map`, 
`Reduce`, and `Tap` methods used in the previous example.

```C# title="Program.cs" linenums="1" hl_lines="45-48"
using System;
using System.Linq;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record InsufficientFundsError(string Message);

public static class Program
{
    public static Result<int, InsufficientFundsError> TryPayForMovies() =>
        // Let's assume that movies cost $25
        new Random().Next(100) switch
        {
            var balance when balance > 25 =>
                (balance - 25)
                    .Pipe(Ok<int, InsufficientFundsError>),
            var balance =>
                new InsufficientFundsError($"Movie Error: balance of ${balance} is too low to pay for movies.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };

    public static Result<int, InsufficientFundsError> TryBuyDinner(int balance) =>
        // assuming dinner costs $50
        balance switch
        {
            var bal when bal >= 50 =>
                (balance - 50)
                    .Pipe(Ok<int, InsufficientFundsError>),
            _ =>
                new InsufficientFundsError($"Dinner Error: balance of ${balance} was too low to pay for dinner.")
                    .Pipe(Error<int, InsufficientFundsError>)
        };

    public static void Main() =>
        Enumerable
            .Range(0, 30)
            .ToList()
            .ForEach(_ =>
                TryPayForMovies()
                    .Bind(TryBuyDinner)
                    .Map(balance => balance.ToString())
                    .Map(strBalance => $"${strBalance}")
                    .Effect(
                        withDollarSign =>
                            Console.WriteLine($"Your balance after paying for dinner and the movies: {withDollarSign}"),
                        err => Console.WriteLine(err.Message)));
}
```

### Result EffectOk and EffectError

Perform an `Effect` when a `Result` is `Ok` or when it is an `Error`.

```C#  title="Program.cs" linenums="1" hl_lines="13 17 21 25"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;
using System;

public static class Program
{
    public static void Main()
    {
        // This will print "value" to the console.
        Ok("value")
            .EffectOk(value => Console.WriteLine(value));

        // This will do nothing since the input value was an Error.
        Error<string>(new Exception("Something bad happened"))
            .EffectOk(value => Console.WriteLine(value));

        // This will do nothing since the input value is Ok
        Ok("value")
            .EffectError(exception => Console.WriteLine(exception.Message));

        // This will print "Something bad happened" since it was an error.
        Error<string>(new Exception("Something bad happened"))
            .EffectError(exception => Console.WriteLine(exception.Message));
    }
}
```

### Result Tap, TapOk, and TapError

If we want to `Tap` into the `Result` and perform some effect without consuming the value, we can use `Tap`, `TapOk`, and `TapError`. With `Tap`, some kind of action must be provided for both the `Ok` and `Error` cases.

For `TapOk` and `TapError`, one or more actions can be provided to occur when the `Result` meets that criteria. This will allow us to only perform actions when the value is `Ok` or `Error` for instance.



```C# title="Program.cs" linenums="1" hl_lines="13 20-22 28"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;
using System;

public static class Program
{
    public static void Main()
    {
        // Actions can be performed when ok and error.
        Ok("value")
            .Tap(ok => Console.WriteLine(ok), exception => Console.WriteLine(exception.Message))
            // The Result is not consumed so we can still use it afterwards.
            .Map(ok => ok + "!");

        // We can do multiple things when the value is Ok with a TapOk.
        string? temp = null;
        Ok("value")
            .TapOk(
                value => Console.WriteLine(value),
                value => temp = value);

        // Nothing happens here since the value is an error.
        // Error<T> creates a Result<T, Exception> by accepting a string or an Exception.
        // The value "Error!" is used as the exception message.
        Error<string>("Error!")
            .TapOk(value => Console.WriteLine(value));
    }
}

```

### Result Unwrap and UnwrapError

If we need to get the value out of a `Result` for some reason and it's impractical to use `Match`, `Map`, `Tap`, or `Effect`, we can `Unwrap` or `UnwrapError` in order to get its inner contents.
It's vital to check to see if the `Result` is `Ok` before using `Unwrap` and see if it's `Error` before using `UnwrapError`, otherwise it will throw an exception!

```C# title="Program.cs" linenums="1" hl_lines="10 13 17-24"
namespace ScratchPad;
using static Functional.Prelude;
using Functional;

public static class Program
{
    public static void Main()
    {
        // This will unwrap fine because the value is Ok.
        string value = Ok("value").Unwrap();

        // This will throw an exception because the value is an error.
        string never = Error<string>("Error!").Unwrap();

        // To do this safely, we need to always check the Option first!
        var result = Error<string>("Error!");
        if (result.IsOk)
        {
            value = result.Unwrap();
        }
        if (result.IsError)
        {
            value = result.UnwrapError().Message;
        }
    }
}
```

### Async Results

Asynchronous support is also provided in this library for `Result`.

Included async methods:

- `MapAsync`
- `ReduceAsync`
- `MatchAsync`
- `BindAsync`
- `EffectAsync`
- `EffectOkAsync`
- `EffectErrorAsync`
- `TapAsync`
- `TapOkAsync`
- `TapErrorAsync`
- `UnwrapAsync`
- `UnwrapErrorAsync`

```C# title="Program.cs" linenums="1"
using System;
using System.Threading.Tasks;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record CustomError(string Message);

public static class Program
{
    public static async Task Main() =>
        await Ok<string, CustomError>("Ok")
            .Async()
            .MapAsync(ok => ok + "!")
            .EffectAsync(
                ok => Console.WriteLine(ok),
                err => Console.WriteLine(err.Message));
}
```

## Common Extensions

Throughout these examples, there have been a few extension methods that have 
made functional programming easier to work with in C#. Let's talk about a few 
of those in detail now.

### Pipe

`Pipe` is a general-purpose mapping function that works with any type. Due to 
limitations and naming conflicts in C#, using `Map` again for this purpose 
was not possible. Because of this, I chose to use the name `Pipe` to match the
F# `|>` pipe operator. Pipe allows us to take the results of a previous function, 
transformation, or other expression and then perform additional transformations
on it. Here is a simple example demonstrating its use. There is also a 
`PipeAsync` method for async processing as well.

```C# title="Program.cs" linenums="1" hl_lines="14"
using System;
using Functional;
using static Functional.Prelude;
namespace ScratchPad;

public record CustomError(string Message);

public static class Program
{
    public static int OneMillion => 1_000_000;

    public static void Main() =>
        OneMillion
            .Pipe(money => string.Format("{0:C}", money))
            .Tap(Console.WriteLine)
            .Ignore();
}
```

### Tap

In the previous example, we used the `Tap` method to perform a side-effect 
the output of the `Pipe` method. The way that `Tap` works, is that it will 
perform operations on the output of the previous value, and then return that 
value as its output. For immutable types, the output will be unchanged. However,
be warned that if the action performed in the `Tap` method mutates the input, 
then the output will also have mutated values. Below is an example with a mutable 
property to demonstrate this behavior. There is also a `TapAsync` method for 
async processing as well.

```C# title="Program.cs" linenums="1" hl_lines="20"
using System;
using Functional;
using static Functional.Prelude;
namespace ScratchPad;

public class IntClass
{
    public int Value { get; set; }
}

public static class Program
{
    public static int OneMillion => 1_000_000;

    public static void Main()
    {
        var classValue =
            OneMillion
                .Pipe(number => new IntClass { Value = number })
                .Tap(intClass => intClass.Value += 1)
                .Pipe(intClass => intClass.Value);

        // Prints "1000001" because the class value was mutated.
        Console.WriteLine(classValue);
    }

}
```

`Tap` allows multiple actions to provided so that many things can be done to 
the input at once. Here is an example demonstrating adding 1 and then printing
the results before saving the value in a variable.

```C# title="Program.cs" linenums="1" hl_lines="20-22"
using System;
using Functional;
using static Functional.Prelude;
namespace ScratchPad;

public class IntClass
{
    public int Value { get; set; }
}

public static class Program
{
    public static int OneMillion => 1_000_000;

    public static void Main()
    {
        var classValue =
            OneMillion
                .Pipe(number => new IntClass { Value = number })
                .Tap(
                    intClass => intClass.Value += 1,
                    intClass => Console.WriteLine($"Printed In Tap: {intClass.Value}"))
                .Pipe(intClass => intClass.Value);

        // Prints "1000001" because the class value was mutated.
        Console.WriteLine($"Printed at the end: {classValue}");
    }
}
```

### Effect

`Effect` is like a `Pipe` that consumes the input, performs some series 
of actions, and returns `Unit`.

```C# title="Program.cs" linenums="1" hl_lines="14 16-17 19-20"
namespace ScratchPad;

using Functional;

using System;

using static Functional.Prelude;

public static class Program
{
    public static void Main()
    {
        "Some Random Value"
            .Effect(input => Console.WriteLine(input));

        Effect(() => Console.WriteLine("another way."))
            .Pipe(unit => "It's a unit type!");

        EffectAsync(() => Console.WriteLine("This one returns a Task<Unit>!"))
            .PipeAsync(unit => "It's another unit!");
    }
}
```

### Cons

`Cons` generates an `ImmutableList` of any type that you put in it. In .NET
8 and C# 12, Collection Expressions and Collection Literals help reduce the 
need for this, but it can still be useful in older versions. See example 
below for usage.

```C# title="Program.cs" linenums="1" hl_lines="10"
using System;
using static Functional.Common.CommonExtensions;

namespace ScratchPad;

public static class Program
{
    public static void Main()
    {
        Cons("some", "things", "to", "print")
            .ForEach(Console.WriteLine);
    }
}
```

### Ignore

`Ignore` and `IgnoreAsync` are used to ignore the output of a function. In languages like F#, any unused values must be explicitly ignored. In C#, this isn't required. To indicate that a calculated result is ignored, you can add this to the end of the function. `Ignore` produces `void` and `IgnoreAsync` produces a `Task`.

```C# title="Program.cs" linenums="1" hl_lines="12 17"
using System.Threading.Tasks;
using static Functional.Common.CommonExtensions;

namespace ScratchPad;

public static class Program
{
    public static async Task Main()
    {
        "Some Contents"
            .Pipe(str => str + "!")
            .Ignore();

        await "Some Async Contents"
            .Async()
            .PipeAsync(str => str + "!")
            .IgnoreAsync();
    }
}
```

## Exception Handling

When interacting with code that can throw Exceptions, we normally reach for the 
traditional `Try/Catch/Finally` block. CSharp.Made.Functional includes a few 
methods to deal with exceptions in a more fluent style.

### Try

Use `Try` to perform an operation which could throw an Exception. There are two 
variants to this method. First, it can be used as a plain static method 
which expects some function to be performed that returns some kind of value. 
There is also an extension method that allows a previous value to be used as 
input to `Try` which is shown in a later example. The return type of these 
methods is `Result<TResult, Exception>` where `TResult` is the type that the 
operation returns. Since there are already a lot of useful methods available 
on `Result`, it makes working with basic `Try/Catch` work simpler. 
CSharp.Made.Functional does not provide any mechanisms for a `Finally` 
block and it's recommended to use the standard `Try/Catch/Finally` approach in 
those cases. Since overly broad Exception catching is not a best practice, 
it is recommended to use a `Switch Expression` when matching on the `Result` 
to handle specific exceptions that are expected for this operation.

In the example below, since we think it could throw, we want to use 
the `Result` type to help us determine if it was `Ok` or an `Error`. We can
make decisions in the `Catch` handler as to whether or not we want to return 
an `Error` or we can also throw if it truly is a catastrophic exception.

Like all of the other methods in this library, there are also async methods 
which work the same way.

```C# title="Program.cs" linenums="1" hl_lines="19-20 32-37"
using System;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record CustomError(string Message);

public static class Program
{
    public static int ItMightThrow() =>
        new Random().Next(100) switch
        {
            var value when value > 50 => value,
            var value => throw new Exception($"Value was {value}")
        };

    public static void Main() => 
        Try(ItMightThrow)
            .Match(
                ok => ok.Pipe(Result.Ok<int, CustomError>),
                exception =>
                {
                    // Example logging.
                    Console.WriteLine(exception.Message);
                    exception
                        .InnerExceptionMessage()
                        .Effect(
                            err => Console.WriteLine(err),
                            () => { /* It was none, don't print anything. */ });

                    return (exception switch
                    {
                        ArgumentNullException => "It was null",
                        OperationCanceledException => "It was cancelled",
                        _ => "We don't know why it crashed..."
                    })
                    .Pipe(msg => new CustomError(msg))
                    .Pipe(Result.Error<int, CustomError>);
                })
            .Match(ok => ok.ToString(), err => err.Message)
            .Tap(Console.WriteLine)
            .Ignore();
}
```

`Try` can also be used as an extension method in order to add a `Try/Catch` 
handler to the end of a function that isn't expected to throw.

```C# title="Program.cs" linenums="1" hl_lines="23 36-41"
using System;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record CustomError(string Message);

public static class Program
{
    public static int ItNeverThrows() =>
        new Random().Next(100);

    public static int ItMightThrow(int input) =>
        input switch
        {
            var value when value > 50 => value,
            var value => throw new Exception($"Value was {value}")
        };

    public static void Main() =>
        ItNeverThrows()
            .Try(ItMightThrow)
            .Match(
                ok => ok.Pipe(Result.Ok<int, CustomError>),
                exception =>
                {
                    // Example logging.
                    Console.WriteLine(exception.Message);
                    exception
                        .InnerExceptionMessage()
                        .Effect(
                            err => Console.WriteLine(err),
                            () => { /* It was none, don't print anything. */ });

                    return (exception switch
                    {
                        ArgumentNullException => "It was null",
                        OperationCanceledException => "It was cancelled",
                        _ => "We don't know why it crashed..."
                    })
                    .Pipe(msg => new CustomError(msg))
                    .Pipe(Result.Error<int, CustomError>);
                })
            .Match(
                ok => ok.ToString(),
                err => err.Message)
            .Tap(Console.WriteLine)
            .Ignore();
}
```

### Inner Exception Messages

Exceptions may or may not have an inner exception message. There is a convenience method called `InnerExceptionMessage()` which returns `Option<string>` to safely handle getting an inner exception method.

```C# title="Program.cs" linenums="1" hl_lines="14 22"
using System;
using Functional;
using static Functional.Prelude;

namespace ScratchPad;

public record CustomError(string Message);

public static class Program
{
    public static void Main()
    {
        new Exception("outer message", new Exception("Inner message"))
            .InnerExceptionMessage()
            .Effect(
                // This will print because there is an inner exception.
                Console.WriteLine,
                () => { /* There was no inner exception */ });

        // Nothing will print here because there was no inner exception.
        new Exception("outer message")
            .InnerExceptionMessage()
            .Effect(
                Console.WriteLine,
                () => { /* There was no inner exception */ });
    }
}
```

### Unit

When performing an action, instead of returning void, we can return the type called `Unit` which represents no return value. This can be used to continue piping more functions after performing an action that would return void. Calling the second `Effect` would not have been possible without the `Unit` type.

```C# title="Program.cs" linenums="1" hl_lines="14 15"
namespace ScratchPad;

using Functional;
using static Functional.Prelude;

using System;

public static class Program
{
    public static void Main()
    {
        // This should print "value" and then "()" on the following line.
        Some("value")
            .Effect(value => Console.WriteLine(value), () => Console.WriteLine("No value"))
            .Effect(unit => Console.WriteLine(unit.ToString()));
    }
}

```