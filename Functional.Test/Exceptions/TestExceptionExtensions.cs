using Functional.Exceptions;
using Functional.Monadic;
using Functional.Options;

namespace Functional.Test.Exceptions;

[TestClass]
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
            .Catch(str => str, exn => exn.Message)
            .ShouldBe(oneMillion.ToString());

    [TestMethod]
    public void TryShouldHandleFailures() =>
        oneMillion
            .Try(_ => ItAlwaysThrows("It threw an exception"))
            .Catch(str => str, exn => exn.Message)
            .ShouldBe("It threw an exception");

    [TestMethod]
    public async Task TryAsyncShouldSucceed() =>
        await oneMillion
            .TryAsync(num => num.ToString().AsAsync())
            .CatchAsync(str => str, exn => exn.Message)
            .TapAsync(str => str.ShouldBe(oneMillion.ToString()));

    [TestMethod]
    public async Task TryAsyncShoulCatch() =>
        await oneMillion
            .TryAsync(_ => ItAlwaysThrows("It threw an exception").AsAsync())
            .CatchAsync(str => str, exn => exn.Message)
            .TapAsync(str => str.ShouldBe("It threw an exception"));

    private static string ItAlwaysThrows(string message) =>
        throw new Exception(message);

    [TestMethod]
    public void ItShouldMatchTrySuccess() =>
        TryResult
            .Success(oneMillion)
            .Match(
                success => success.ToString(),
                exn => exn.Message)
            .ShouldBe("1000000");

    [TestMethod]
    public void ItShouldMatchTryFailure() =>
        TryResult
            .Failure<int>(new Exception("error"))
            .Match(
                success => success.ToString(),
                exn => exn.Message)
            .ShouldBe("error");

    [TestMethod]
    public async Task ITShouldMatchSuccessAsync() =>
        await TryResult
            .Success(oneMillion)
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                exn => exn.Message)
            .TapAsync(res => res.ShouldBe("1000000"));

    [TestMethod]
    public async Task ItShouldMatchFailureAsync() =>
        await TryResult
            .Failure<int>(new Exception("error"))
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                exn => exn.Message)
            .TapAsync(res => res.ShouldBe("error"));

}