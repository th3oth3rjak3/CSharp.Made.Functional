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
            .ShouldBe(Option.None<string>());

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

}
