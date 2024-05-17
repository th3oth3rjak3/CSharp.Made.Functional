namespace Functional.Test.Common;

[ExcludeFromCodeCoverage]
[TestClass]
public class OldCommonTests
{














    [TestMethod]
    public void EffectShouldHandleActionsWithInput()
    {
        var effectResult = string.Empty;

        "input"
            .Effect(input => effectResult = input)
            .ShouldBeOfType<Unit>();

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public void EffectShouldHandleActionsWithNoInput()
    {
        var effectResult = string.Empty;

        "ignored"
            .Effect(() => effectResult = "input")
            .ShouldBeOfType<Unit>();

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithInput()
    {
        var effectResult = string.Empty;

        await "input"
            .Async()
            .EffectAsync(input => effectResult = input)
            .EffectAsync(unit => unit.ShouldBeOfType<Unit>());

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithNoInput()
    {
        var list = new List<int>();

        await "ignored"
            .Async()
            .EffectAsync(
                () => list.Add(1),
                () => list.Add(2))
            .EffectAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task EffectShouldHandleActionsWithNoInputAndTaskOutput()
    {
        var list = new List<int>();

        async Task addListItem(int input) => await input.Async().EffectAsync(input => list.Add(input));

        await "ignored input"
            .Async()
            .EffectAsync(() => addListItem(1), () => addListItem(2));

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task EffectShouldHandleActionsWithInputAndTaskOutput()
    {
        var list = new List<int>();

        async Task addListItem(int input) => await input.Async().EffectAsync(input => list.Add(input));

        await 1
            .Async()
            .EffectAsync(value => addListItem(value), value => addListItem(value + 1));

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }


    [TestMethod]
    public void EffectShouldWorkWithoutExtensionMethod()
    {
        var effectResult = false;
        Effect(() => effectResult = true).ShouldBeOfType<Unit>();
        effectResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithActionInput()
    {
        var effectResult = false;

        await EffectAsync(() => effectResult = true)
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        effectResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithFuncTaskInput()
    {
        var effectResult = false;

        Task doWork() => Effect(() => effectResult = true).Async();

        await EffectAsync(doWork)
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        effectResult.ShouldBeTrue();
    }
}
