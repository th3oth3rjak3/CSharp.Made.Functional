namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class BindTests
{
    private static Result<int, Exception> BindingFunction(int input) =>
        input < 20
            ? input
            : new Exception("Value too high");

    private static Task<Result<int, Exception>> BindingFunctionAsync(int input) =>
        Task.Run(() => BindingFunction(input));


    [TestMethod]
    public async Task ItShouldBindAsync_1()
    {
        await Success(10)
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsSuccess.ShouldBeTrue();
            });

        await Success(30)
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsSuccess.ShouldBeFalse();
                output.UnwrapFailure().Message.ShouldBe("Value too high");
            });

        await Failure<int>(new Exception("Initial failure"))
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsFailure.ShouldBeTrue();
                output.UnwrapFailure().Message.ShouldBe("Initial failure");
            });
    }

    [TestMethod]
    public async Task ItShouldBindAsync_2()
    {
        await Success(10)
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsSuccess.ShouldBeTrue();
            });

        await Success(30)
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsSuccess.ShouldBeFalse();
                output.UnwrapFailure().Message.ShouldBe("Value too high");
            });

        await Failure<int>(new Exception("Initial failure"))
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsFailure.ShouldBeTrue();
                output.UnwrapFailure().Message.ShouldBe("Initial failure");
            });
    }
}