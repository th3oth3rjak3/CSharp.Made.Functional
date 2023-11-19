using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Exceptions;
using Functional.Options;

using static Functional.Exceptions.TryCatch;

namespace Functional.Test.Exceptions;

[TestClass]
[ExcludeFromCodeCoverage]
public class TestExceptionExtensions
{
    private const int oneMillion = 1_000_000;

    [TestMethod]
    public void ItShouldGetInnerExceptionMessage() =>
        new Exception("outer message", new Exception("inner message"))
            .InnerExceptionMessage()
            .Reduce("wasn't found")
            .ShouldBe("inner message");

    [TestMethod]
    public void ItShouldNotGetInnerExceptionMessage() =>
        new Exception("outer message")
            .InnerExceptionMessage()
            .ShouldBeEquivalentTo(Option.None<string>());

    [TestMethod]
    public void TryShouldSucceed() =>
        oneMillion
            .Try(num => num.ToString())
            .Catch(exn => exn.Message)
            .Invoke()
            .ShouldBe(oneMillion.ToString());

    [TestMethod]
    public void TryShouldHandleFailures() =>
        oneMillion
            .Try(_ => ItAlwaysThrows("It threw an exception"))
            .Catch(exn => exn.Message)
            .Invoke()
            .ShouldBe("It threw an exception");

    [TestMethod]
    public async Task TryAsyncShouldSucceed() =>
        await oneMillion
            .AsAsync()
            .TryAsync(num => num.ToString().AsAsync())
            .CatchAsync(exn => exn.Message.AsAsync())
            .InvokeAsync()
            .TapAsync(str => str.ShouldBe(oneMillion.ToString()));

    [TestMethod]
    public async Task TryAsyncShouldCatch() =>
        await oneMillion
            .AsAsync()
            .TryAsync(_ => ItAlwaysThrows("It threw an exception").AsAsync())
            .CatchAsync(exn => exn.Message.AsAsync())
            .InvokeAsync()
            .TapAsync(str => str.ShouldBe("It threw an exception"));

    private static string ItAlwaysThrows(string message) =>
        throw new Exception(message);

    [DataRow(1)]
    [TestMethod]
    public void ItShouldTryWithClosures(int someNumber) =>
        Try(() => someNumber.ToString())
            .Catch(exn => exn.Message)
            .Invoke()
            .ShouldBe("1");

    [DataRow(1)]
    [TestMethod]
    public void ItShouldCatchExceptionsWithClosures(int someNumber) =>
        Try(() => AlwaysThrows(someNumber))
            .Catch(exn => exn.Message)
            .Invoke()
            .ShouldBe("Something bad happened.");

    public static string AlwaysThrows(int number) =>
        throw new Exception("Something bad happened.");

    [TestMethod]
    public async Task ItShouldTryCatchAsync() =>
        await TryAsync(() => ItAlwaysThrows("never").AsAsync())
            .CatchAsync(exn => exn.Message.Pipe(msg => $"Exception: {msg}").AsAsync())
            .InvokeAsync()
            .TapAsync(msg => msg.ShouldBe("Exception: never"));

    [TestMethod]
    public void ItShouldTryCatchFinally()
    {
        var invocations = 0;

        Try(() => "success")
            .Catch(exn => exn.Message)
            .Finally(() => invocations++)
            .Invoke()
            .ShouldBe("success");

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public void ItShouldTryCatchFinallyException()
    {
        var invocations = 0;

        Try<string>(() => throw new Exception("exception"))
            .Catch(exn => exn.Message)
            .Finally(() => invocations++)
            .Invoke()
            .ShouldBe("exception");

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public void ItShouldTryCatchFinallyWithInput()
    {
        var invocations = 0;

        oneMillion
            .Try(value => value.ToString())
            .Catch(exn => exn.Message)
            .Finally(() => invocations++)
            .Invoke()
            .ShouldBe(oneMillion.ToString());

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public async Task ItShouldTryCatchFinallyWithInputAsync()
    {
        var invocations = 0;

        _ = await oneMillion
            .AsAsync()
            .TryAsync(value => value.ToString().AsAsync())
            .CatchAsync(exn => exn.Message.AsAsync())
            .FinallyAsync(() => invocations++)
            .InvokeAsync()
            .TapAsync(value => value.ShouldBe(oneMillion.ToString()));

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public void ItShouldTryCatchFinallyWithInputException()
    {
        var invocations = 0;

        oneMillion
            .Try<int, string>(value => throw new Exception("exception"))
            .Catch(exn => exn.Message)
            .Finally(() => invocations++)
            .Invoke()
            .ShouldBe("exception");

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public async Task ItShouldTryCatchFinallyWithInputExceptionAsync()
    {
        var invocations = 0;

        _ = await oneMillion
            .AsAsync()
            .TryAsync<int, string>(value => throw new Exception("exception"))
            .CatchAsync(exn => exn.Message.AsAsync())
            .FinallyAsync(() => invocations++)
            .InvokeAsync()
            .TapAsync(value => value.ShouldBe("exception"));

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public async Task ItShouldTryCatchFinallyAsync()
    {
        var invocations = 0;

        await TryAsync(() => "message".AsAsync())
            .CatchAsync(exn => exn.Message.AsAsync())
            .FinallyAsync(() => invocations++)
            .InvokeAsync()
            .TapAsync(value => value.ShouldBe("message"))
            .IgnoreAsync();

        invocations.ShouldBe(1);
    }

    [TestMethod]
    public async Task ItShouldTryCatchFinallyExceptionAsync()
    {
        var invocations = 0;

        await TryAsync<string>(() => throw new Exception("exception"))
            .CatchAsync(exn => exn.Message.AsAsync())
            .FinallyAsync(() => invocations++)
            .InvokeAsync()
            .TapAsync(value => value.ShouldBe("exception"))
            .IgnoreAsync();

        invocations.ShouldBe(1);
    }
}