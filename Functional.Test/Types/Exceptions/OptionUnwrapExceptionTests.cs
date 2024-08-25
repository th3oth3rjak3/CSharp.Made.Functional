namespace Functional.Test.Types.Exceptions;
[TestClass]
[ExcludeFromCodeCoverage]
public class OptionUnwrapExceptionTests
{
    [TestMethod]
    public void ItShouldProduceAnErrorMessage() =>
        new OptionUnwrapException()
            .Message
            .ShouldBe("An option was unwrapped when the value was None. Be sure to check the option first with 'IsSome'.");
}
