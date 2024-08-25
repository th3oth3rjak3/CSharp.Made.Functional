namespace Functional.Test.Types.Exceptions;
[TestClass]
[ExcludeFromCodeCoverage]
public class ResultUnwrapErrorExceptionTests
{
    [TestMethod]
    public void ItShouldProduceAnErrorMessage() => new ResultUnwrapErrorException()
            .Message
            .ShouldBe("A result was unwrapped as an error when the value was Ok. Be sure to check the result first with 'IsError'.");
}
