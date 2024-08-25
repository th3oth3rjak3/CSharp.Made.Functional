namespace Functional.Test.Types.Exceptions;
[ExcludeFromCodeCoverage]
[TestClass]
public class ResultUnwrapExceptionTests
{
    [TestMethod]
    public void ItShouldProduceAnErrorMessage() => new ResultUnwrapException()
            .Message
            .ShouldBe("A result was unwrapped when the value was an Error. Be sure to check the result first with 'IsOk'.");
}
