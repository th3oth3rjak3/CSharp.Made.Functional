﻿namespace Functional.Test.Prelude;

[TestClass]
[ExcludeFromCodeCoverage]
public class ExceptionTests
{
    private const int OneMillion = 1_000_000;

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
            .ShouldBeEquivalentTo(None<string>());

    [TestMethod]
    public void TryShouldSucceed() =>
        OneMillion
            .Try(num => num.ToString())
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe(OneMillion.ToString());

    [TestMethod]
    public void TryShouldHandleErrors() =>
        OneMillion
            .Try(_ => ItAlwaysThrows("It threw an exception"))
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("It threw an exception");

    [TestMethod]
    public async Task TryAsyncShouldSucceed() =>
        await OneMillion
            .Async()
            .TryAsync(num => num.ToString().Async())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(str => str.ShouldBe(OneMillion.ToString()));

    [TestMethod]
    public async Task TryAsyncShouldCatch() =>
        await OneMillion
            .Async()
            .TryAsync(_ => ItAlwaysThrows("It threw an exception").Async())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(str => str.ShouldBe("It threw an exception"));

    private static string ItAlwaysThrows(string message) =>
        throw new Exception(message);

    [DataRow(1)]
    [TestMethod]
    public void ItShouldTryWithClosures(int someNumber) =>
        Try(someNumber.ToString)
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("1");

    [DataRow(1)]
    [TestMethod]
    public void ItShouldCatchExceptionsWithClosures(int someNumber) =>
        Try(() => AlwaysThrows(someNumber))
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("Something bad happened.");

    private static string AlwaysThrows(int number) =>
        throw new Exception("Something bad happened.");

    [TestMethod]
    public async Task ItShouldTryCatchAsync() =>
        await TryAsync(() => ItAlwaysThrows("never").Async())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(msg => msg.ShouldBe("never"));

    [TestMethod]
    public async Task ItShouldTryCatchWithoutThrowingAsync() =>
        await TryAsync(() => "success".Async())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(msg => msg.ShouldBe("success"));

    [TestMethod]
    public void ItShouldTryWithAction()
    {
        var output = string.Empty;
        Try(ItThrows)
            .Effect(
                _ => throw new ShouldAssertException("it should have been error"),
                err => err.Message.ShouldBe("error"));

        output.ShouldBeEmpty();

        Try(ItDoesntThrow)
            .Effect(
                ok => ok.ShouldBe(Unit()),
                _ => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("success");
        
        return;
        void ItThrows() => throw new Exception("error");
        void ItDoesntThrow() => output = "success";
    }

    [TestMethod]
    public void ItShouldTryWithActionT()
    {
        var output = string.Empty;
        "input"
            .Try(ItThrows)
            .Effect(
                _ => throw new ShouldAssertException("it should have been error"),
                err => err.Message.ShouldBe("error"));

        output.ShouldBeEmpty();
        
        "input"
            .Try(ItDoesntThrow)
            .Effect(
                ok => ok.ShouldBe(Unit()),
                _ => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("input");
        
        return;
        void ItThrows(string input) => throw new Exception("error");
        void ItDoesntThrow(string input) => output = input;
    }

    [TestMethod]
    public async Task TryAsyncShouldWorkWithActions()
    {
        await TryAsync(ItThrows)
            .EffectAsync(
                ok => throw new ShouldAssertException("it should have been error"),
                err => err.Message.ShouldBe("error"));

        var output = string.Empty;

        await TryAsync(ItDoesntThrow)
            .EffectAsync(
                ok => ok.ShouldBe(Unit()),
                err => throw new ShouldAssertException("It should have been ok"));
        
        output.ShouldBe("success");

        return;

        void ItThrows() => throw new Exception("error");
        void ItDoesntThrow() => output = "success";
    }

    [TestMethod]
    public async Task TryAsyncShouldWorkWithActionT()
    {
        var output = string.Empty;

        await "input"
            .Async()
            .TryAsync(ItThrows)
            .EffectAsync(
                ok => throw new ShouldAssertException("It should have been error"),
                err => err.Message.ShouldBe("error"));

        await "input"
            .Async()
            .TryAsync(ItDoesntThrow)
            .EffectAsync(
                ok => ok.ShouldBe(Unit()),
                err => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("input");

        return;
        void ItThrows(string input) => throw new Exception("error");
        void ItDoesntThrow(string input) => output = input;
    }
}