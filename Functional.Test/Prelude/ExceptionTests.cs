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

        Try(Safe)
            .Effect(
                ok => ok.ShouldBe(Unit()),
                _ => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("success");

        return;
        void ItThrows() => throw new Exception("error");
        void Safe() => output = "success";
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
            .Try(Safe)
            .Effect(
                ok => ok.ShouldBe(Unit()),
                _ => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("input");

        return;
        void ItThrows(string input) => throw new Exception("error");
        void Safe(string input) => output = input;
    }

    [TestMethod]
    public async Task TryAsyncShouldWorkWithActions()
    {
        await TryAsync(ItThrows)
            .EffectAsync(
                ok => throw new ShouldAssertException("it should have been error"),
                err => err.Message.ShouldBe("error"));

        var output = string.Empty;

        await TryAsync(Safe)
            .EffectAsync(
                ok => ok.ShouldBe(Unit()),
                err => throw new ShouldAssertException("It should have been ok"));

        output.ShouldBe("success");

        return;

        void ItThrows() => throw new Exception("error");
        void Safe() => output = "success";
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
            .TryAsync(Safe)
            .EffectAsync(
                ok => ok.ShouldBe(Unit()),
                err => throw new ShouldAssertException("it should have been ok"));

        output.ShouldBe("input");

        return;
        void ItThrows(string input) => throw new Exception("error");
        void Safe(string input) => output = input;
    }

    [TestMethod]
    public void TryMapShouldTryToMapOptions_1()
    {
        Some(1)
            .TryMap(ItThrows)
            .TapError(error => error.Message.ShouldBe("it threw"))
            .IsError.ShouldBeTrue();

        None<int>()
            .TryMap(ItThrows)
            // True because it won't map when the input is a None.
            .IsOk.ShouldBeTrue();

        var option =
            Some(1)
                .TryMap(Safe)
                .Unwrap();
        option.Unwrap().ShouldBe("1");

        option = None<int>()
            .TryMap(Safe)
            .Unwrap();

        option.IsNone.ShouldBeTrue();

        return;

        string ItThrows(int input) => throw new Exception("it threw");
        string Safe(int input) => input.ToString();
    }

    [TestMethod]
    public void TryMapShouldTryToMapOptions_2()
    {
        Some(1)
            .TryMap(ItThrows)
            .TapError(error => error.Message.ShouldBe("it throws"))
            .IsError
            .ShouldBeTrue();

        Some(1)
            .TryMap(ItMaps)
            .Unwrap()
            .Unwrap()
            .ShouldBe("42");

        None<int>()
            .TryMap(ItThrows)
            .TapOk(ok => ok.IsNone.ShouldBeTrue())
            .IsOk
            .ShouldBeTrue();

        None<int>()
            .TryMap(ItMaps)
            .TapOk(ok => ok.IsNone.ShouldBeTrue())
            .IsOk
            .ShouldBeTrue();

        return;

        string ItThrows() => throw new Exception("it throws");
        string ItMaps() => "42";
    }

    [TestMethod]
    public async Task OptionTryMapAsync_1()
    {
        var mapped =
            await Some("maybe an integer")
                .Async()
                .TryMapAsync(int.Parse);

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Some("42")
                .Async()
                .TryMapAsync(int.Parse);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe(42);

        mapped =
            await None<string>()
                .Async()
                .TryMapAsync(int.Parse);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task OptionTryMapAsync_2()
    {
        var mapped =
            await Some("100")
                .Async()
                .TryMapAsync(() => int.Parse("it won't work"));

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Some("input doesn't matter here")
                .Async()
                .TryMapAsync(() => int.Parse("42"));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe(42);

        mapped =
            await None<string>()
            .Async()
            .TryMapAsync(() => int.Parse("it doesn't matter because the input is none."));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task OptionTryMapAsync_3()
    {
        var mapped =
            await Some("error")
                .Async()
                .TryMapAsync(value => int.Parse(value).Async());

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Some("42")
                .Async()
                .TryMapAsync(value => int.Parse(value).Async());

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe(42);

        mapped =
            await None<string>()
                .Async()
                .TryMapAsync(value => int.Parse("doesn't matter").Async());

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task OptionTryMapAsync_4()
    {
        var mapped =
            await Some("100")
                .Async()
                .TryMapAsync(() => int.Parse("won't parse").Async());

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Some("anything")
                .Async()
                .TryMapAsync(() => int.Parse("42"));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe(42);

        mapped =
            await None<string>()
                .Async()
                .TryMapAsync(() => int.Parse("won't matter because input is None"));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public void OptionTryBind_1()
    {
        var mapped =
            Some(8)
                .TryBind(UnsafeToString);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too low");

        mapped =
            Some(42)
            .TryBind(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        mapped =
            Some(15)
            .TryBind(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe("15");

        mapped =
            None<int>()
            .TryBind(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        return;

        static Option<string> UnsafeToString(int input) =>
            input < 10
                ? throw new Exception("value too low")
                : input > 30
                    ? None<string>()
                    : Some(input.ToString());
    }

    [TestMethod]
    public void OptionTryBind_2()
    {
        Some(10)
            .TryBind(ToString)
            .Unwrap()
            .Unwrap()
            .ShouldBe("value");

        Some(10)
            .TryBind(NoneToString)
            .Unwrap()
            .IsNone
            .ShouldBeTrue();

        Some(10)
            .TryBind(Unsafe)
            .IsError
            .ShouldBeTrue();

        None<int>()
            .TryBind(ToString)
            .Unwrap()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        None<int>()
            .TryBind(NoneToString)
            .Unwrap()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        None<int>()
            .TryBind(Unsafe)
            .Unwrap()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        return;

        Option<string> ToString() =>
            Some("value");

        Option<string> NoneToString() =>
            None<string>();

        Option<string> Unsafe() =>
            throw new Exception("error");
    }

    [TestMethod]
    public async Task OptionTryBindAsync_1()
    {
        var mapped =
            await Some(1)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too low");

        mapped =
            await Some(40)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        mapped =
            await Some(15)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe("15");

        mapped =
            await None<int>()
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        return;

        static Option<string> UnsafeToString(int input) =>
            input < 10
            ? throw new Exception("value too low")
            : input > 30
                ? None<string>()
                : Some(input.ToString());
    }

    [TestMethod]
    public async Task OptionTryBindAsync_2()
    {
        var mapped =
            await Some(42)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("error");

        mapped =
            await Some(42)
            .Async()
            .TryBindAsync(GetString);

        mapped.Unwrap().Unwrap().ShouldBe("value");

        mapped =
            await Some(42)
            .Async()
            .TryBindAsync(NoneString);

        mapped.Unwrap().IsNone.ShouldBeTrue();

        mapped =
            await None<int>()
            .Async()
            .TryBindAsync(Unsafe);

        mapped
            .Unwrap()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        return;

        Option<string> Unsafe() => throw new Exception("error");
        Option<string> GetString() => Some("value");
        Option<string> NoneString() => None<string>();
    }

    [TestMethod]
    public async Task OptionTryBindAsync_3()
    {
        var mapped =
            await Some(1)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too low");

        mapped =
            await Some(40)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        mapped =
            await Some(15)
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().Unwrap().ShouldBe("15");

        mapped =
            await None<int>()
            .Async()
            .TryBindAsync(UnsafeToString);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().IsNone.ShouldBeTrue();

        return;

        static Task<Option<string>> UnsafeToString(int input) =>
            input < 10
            ? throw new Exception("value too low")
            : input > 30
                ? None<string>().Async()
                : Some(input.ToString()).Async();
    }

    [TestMethod]
    public async Task OptionTryBindAsync_4()
    {
        var mapped =
            await Some(42)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("error");

        mapped =
            await Some(42)
            .Async()
            .TryBindAsync(GetString);

        mapped.Unwrap().Unwrap().ShouldBe("value");

        mapped =
            await Some(42)
            .Async()
            .TryBindAsync(NoneString);

        mapped.Unwrap().IsNone.ShouldBeTrue();

        mapped =
            await None<int>()
            .Async()
            .TryBindAsync(Unsafe);

        mapped
            .Unwrap()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        return;

        Task<Option<string>> Unsafe() => throw new Exception("error");
        Task<Option<string>> GetString() => Some("value").Async();
        Task<Option<string>> NoneString() => None<string>().Async();
    }

    [TestMethod]
    public void ResultTryMap_1()
    {
        var mapped =
            Ok("not an integer")
                .TryMap(int.Parse);

        mapped.IsError.ShouldBeTrue();

        mapped =
            Ok("42")
            .TryMap(int.Parse);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe(42);

        mapped =
            Error<string>("an existing error")
            .TryMap(int.Parse);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("an existing error");
    }

    [TestMethod]
    public void ResultTryMap_2()
    {
        var mapped =
            Ok("100")
                .TryMap(() => int.Parse("something that won't parse"));

        mapped.IsError.ShouldBeTrue();

        mapped =
            Ok("42")
            .TryMap(() => int.Parse("100"));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe(100);

        mapped =
            Error<string>("an error occurred")
                .TryMap(() => int.Parse("it shouldn't matter"));

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("an error occurred");
    }

    [TestMethod]
    public async Task ResultTryMapAsync_1()
    {
        var mapped =
            await Ok("not an integer")
            .Async()
            .TryMapAsync(int.Parse);

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Ok("42")
            .Async()
            .TryMapAsync(int.Parse);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe(42);

        mapped =
            await Error<string>("error")
            .Async()
            .TryMapAsync(int.Parse);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("error");
    }

    [TestMethod]
    public async Task ResultTryMapAsync_2()
    {
        var mapped =
            await Ok("100")
            .Async()
            .TryMapAsync(() => int.Parse("failure"));

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Ok("failure")
            .Async()
            .TryMapAsync(() => int.Parse("100"));

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe(100);

        mapped =
            await Error<string>("an error")
            .Async()
            .TryMapAsync(() => int.Parse("it doesn't matter"));

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("an error");
    }

    [TestMethod]
    public async Task ResultTryMapAsync_3()
    {
        var mapped =
            await Ok("not an integer")
            .Async()
            .TryMapAsync(value => int.Parse(value).Async());

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Ok("42")
            .Async()
            .TryMapAsync(value => int.Parse(value).Async());

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe(42);

        mapped =
            await Error<string>("an error")
            .Async()
            .TryMapAsync(value => int.Parse(value).Async());

        mapped.UnwrapError().Message.ShouldBe("an error");
    }

    [TestMethod]
    public async Task ResultTryMapAsync_4()
    {
        var mapped =
            await Ok("100")
            .Async()
            .TryMapAsync(() => int.Parse("something that won't parse").Async());

        mapped.IsError.ShouldBeTrue();

        mapped =
            await Ok("random")
            .Async()
            .TryMapAsync(() => int.Parse("100").Async());

        mapped.Unwrap().ShouldBe(100);

        mapped =
            await Error<string>("an error")
            .Async()
            .TryMapAsync(() => int.Parse("anything").Async());

        mapped.UnwrapError().Message.ShouldBe("an error");
    }

    [TestMethod]
    public void ResultTryBind_1()
    {
        var mapped =
            Ok(8).TryBind(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too low");


        mapped =
            Ok(42).TryBind(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too high");

        mapped =
            Ok(15).TryBind(Unsafe);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe("15");

        mapped =
            Error<int>("an error").TryBind(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("an error");

        return;

        static Result<string, Exception> Unsafe(int input) =>
            input < 10
            ? throw new Exception("value too low")
            : input > 30
                ? Error<string>("value too high")
                : Ok(input.ToString());
    }

    [TestMethod]
    public void ResultTryBind_2()
    {
        var mapped = Ok(8).TryBind(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("error");

        mapped =
            Ok(42).TryBind(ErrorString);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("something bad happened");

        mapped =
            Ok(15).TryBind(GetString);

        mapped.Unwrap().ShouldBe("value");

        mapped =
            Error<string>("original error").TryBind(GetString);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("original error");

        return;

        static Result<string, Exception> Unsafe() => throw new Exception("error");
        static Result<string, Exception> GetString() => Ok("value");
        static Result<string, Exception> ErrorString() => Error<string>("something bad happened");
    }

    [TestMethod]
    public async Task ResultTryBindAsync_1()
    {
        var mapped =
            await Ok(8).Async().TryBindAsync(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too low");


        mapped =
            await Ok(42).Async().TryBindAsync(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("value too high");

        mapped =
            await Ok(15).Async().TryBindAsync(Unsafe);

        mapped.IsOk.ShouldBeTrue();
        mapped.Unwrap().ShouldBe("15");

        mapped =
            await Error<int>("an error").Async().TryBindAsync(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("an error");

        return;

        static Result<string, Exception> Unsafe(int input) =>
            input < 10
            ? throw new Exception("value too low")
            : input > 30
                ? Error<string>("value too high")
                : Ok(input.ToString());
    }

    [TestMethod]
    public async Task ResultTryBindAsync_2()
    {
        var mapped =
            await Ok(8)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.IsError.ShouldBeTrue();
        mapped.UnwrapError().Message.ShouldBe("error");

        mapped =
            await Ok(42)
            .Async()
            .TryBindAsync(ErrorString);

        mapped.UnwrapError().Message.ShouldBe("an error");

        mapped =
            await Ok(15)
            .Async()
            .TryBindAsync(GetString);

        mapped.Unwrap().ShouldBe("value");

        mapped =
            await Error<string>("original error")
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("original error");

        return;

        static Result<string, Exception> Unsafe() => throw new Exception("error");
        static Result<string, Exception> GetString() => "value";
        static Result<string, Exception> ErrorString() => Error<string>("an error");
    }

    [TestMethod]
    public async Task ResultTryBindAsync_3()
    {
        var mapped =
            await Ok(8)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("value too low");

        mapped =
            await Ok(42)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("value too high");

        mapped =
            await Ok(15)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.Unwrap().ShouldBe("15");

        mapped =
            await Error<int>("original error")
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("original error");

        return;

        static Task<Result<string, Exception>> Unsafe(int input) =>
            input < 10
            ? throw new Exception("value too low")
            : input > 30
                ? Error<string>("value too high").Async()
                : Ok(input.ToString()).Async();
    }

    [TestMethod]
    public async Task ResultTryBindAsync_4()
    {
        var mapped =
            await Ok(8)
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("error");

        mapped =
            await Ok(42)
            .Async()
            .TryBindAsync(ErrorString);

        mapped.UnwrapError().Message.ShouldBe("something bad happened");

        mapped =
            await Ok(15)
            .Async()
            .TryBindAsync(GetString);

        mapped.Unwrap().ShouldBe("value");

        mapped =
            await Error<int>("original error")
            .Async()
            .TryBindAsync(Unsafe);

        mapped.UnwrapError().Message.ShouldBe("original error");

        return;

        static Task<Result<string, Exception>> Unsafe() => throw new Exception("error");
        static Task<Result<string, Exception>> GetString() => Ok("value").Async();
        static Task<Result<string, Exception>> ErrorString() => Error<string>("something bad happened").Async();
    }
}