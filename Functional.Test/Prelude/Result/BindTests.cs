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
        await Ok(10)
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsOk.ShouldBeTrue();
            });

        await Ok(30)
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsOk.ShouldBeFalse();
                output.UnwrapError().Message.ShouldBe("Value too high");
            });

        await Error<int>(new Exception("Initial Error"))
            .Async()
            .BindAsync(BindingFunction)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsError.ShouldBeTrue();
                output.UnwrapError().Message.ShouldBe("Initial Error");
            });
    }

    [TestMethod]
    public async Task ItShouldBindAsync_2()
    {
        await Ok(10)
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsOk.ShouldBeTrue();
            });

        await Ok(30)
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsOk.ShouldBeFalse();
                output.UnwrapError().Message.ShouldBe("Value too high");
            });

        await Error<int>(new Exception("Initial Error"))
            .Async()
            .BindAsync(BindingFunctionAsync)
            .EffectAsync(output =>
            {
                output.AssertInstanceOfType(typeof(Result<int, Exception>));
                output.IsError.ShouldBeTrue();
                output.UnwrapError().Message.ShouldBe("Initial Error");
            });
    }

    [TestMethod]
    public async Task ItShouldBindAsync_3()
    {
        var mapped =
            await Ok(10)
            .Async()
            .BindAsync(() => Ok("else"));

        mapped.Unwrap().ShouldBe("else");

        mapped =
            await Ok(10)
            .Async()
            .BindAsync(() => Error<string>("an error"));

        mapped.UnwrapError().Message.ShouldBe("an error");

        mapped =
            await Error<int>("original error")
            .Async()
            .BindAsync(() => Ok("anything"));

        mapped.UnwrapError().Message.ShouldBe("original error");
    }

    [TestMethod]
    public async Task ItShouldBindAsync_4()
    {
        var mapped =
            await Ok(10)
            .Async()
            .BindAsync(() => Ok("else").Async());

        mapped.Unwrap().ShouldBe("else");

        mapped =
            await Ok(10)
            .Async()
            .BindAsync(() => Error<string>("an error").Async());

        mapped.UnwrapError().Message.ShouldBe("an error");

        mapped =
            await Error<int>("original error")
            .Async()
            .BindAsync(() => Ok("anything").Async());

        mapped.UnwrapError().Message.ShouldBe("original error");
    }
}