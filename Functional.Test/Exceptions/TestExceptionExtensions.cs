using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Exceptions;
using Functional.Options;
using Functional.Results;

using static Functional.Exceptions.ExceptionExtensions;

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
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe(oneMillion.ToString());

    [TestMethod]
    public void TryShouldHandleFailures() =>
        oneMillion
            .Try(_ => ItAlwaysThrows("It threw an exception"))
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("It threw an exception");

    [TestMethod]
    public async Task TryAsyncShouldSucceed() =>
        await oneMillion
            .AsAsync()
            .TryAsync(num => num.ToString().AsAsync())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(str => str.ShouldBe(oneMillion.ToString()));

    [TestMethod]
    public async Task TryAsyncShouldCatch() =>
        await oneMillion
            .AsAsync()
            .TryAsync(_ => ItAlwaysThrows("It threw an exception").AsAsync())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(str => str.ShouldBe("It threw an exception"));

    private static string ItAlwaysThrows(string message) =>
        throw new Exception(message);

    [DataRow(1)]
    [TestMethod]
    public void ItShouldTryWithClosures(int someNumber) =>
        Try(() => someNumber.ToString())
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("1");

    [DataRow(1)]
    [TestMethod]
    public void ItShouldCatchExceptionsWithClosures(int someNumber) =>
        Try(() => AlwaysThrows(someNumber))
            .Match(ok => ok, exn => exn.Message)
            .ShouldBe("Something bad happened.");

    public static string AlwaysThrows(int number) =>
        throw new Exception("Something bad happened.");

    [TestMethod]
    public async Task ItShouldTryCatchAsync() =>
        await TryAsync(() => ItAlwaysThrows("never").AsAsync())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(msg => msg.ShouldBe("never"));

    [TestMethod]
    public async Task ItShouldTryCatchWithoutThrowingAsync() =>
        await TryAsync(() => "success".AsAsync())
            .MatchAsync(ok => ok, exn => exn.Message)
            .TapAsync(msg => msg.ShouldBe("success"));

}