namespace Functional.Test.Prelude;

[ExcludeFromCodeCoverage]
[TestClass]
public class EffectTests
{
    private List<string> EffectResult { get; set; } = [];

    [TestInitialize]
    public void Reset() => EffectResult.Clear();

    private Task PerformEffectAsync(string input) => EffectAsync(() => EffectResult.Add(input)).IgnoreAsync();
    private void PerformEffect(string input) => EffectResult.Add(input);

    [TestMethod]
    public void EffectShouldWorkWithoutExtensionMethod()
    {
        var effectResult = false;
        Effect(() => effectResult = true).ShouldBeOfType<Unit>();
        effectResult.ShouldBeTrue();
    }

    [TestMethod]
    public void EffectShouldHandleMultipleActionsWithInput()
    {
        "input"
            .Effect(
                EffectResult.Add,
                value => EffectResult.Add($"{value}!"))
            .AssertInstanceOfType(typeof(Unit));

        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");
    }

    [TestMethod]
    public void EffectShouldHandleMultipleActionsWithoutInput()
    {
        "ignored"
            .Effect(
                () => EffectResult.Add("1"),
                () => EffectResult.Add("2"))
            .AssertInstanceOfType(typeof(Unit));

        EffectResult.ShouldContain("1");
        EffectResult.ShouldContain("2");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithInput()
    {
        await "input"
            .Async()
            .EffectAsync([
                PerformEffectAsync,
                input => PerformEffectAsync(input + "!")])
            .EffectAsync([
                    unit => unit.ShouldBeOfType<Unit>(),
                    unit => unit.ShouldBeOfType<Unit>()
                ]);

        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithNoInput()
    {
        await "ignored"
            .Async()
            .EffectAsync(
                Cons(
                    () => PerformEffect("1"),
                    () => PerformEffect("2")
                ))
            .EffectAsync([unit => unit.ShouldBeOfType<Unit>()]);

        EffectResult.ShouldContain("1");
        EffectResult.ShouldContain("2");
    }

    [TestMethod]
    public async Task EffectShouldHandleActionsWithNoInputAndTaskOutput()
    {
        await "ignored input"
            .Async()
            .EffectAsync(
                Cons(
                    () => PerformEffectAsync("1"),
                    () => PerformEffectAsync("2")
                ));

        EffectResult.ShouldContain("1");
        EffectResult.ShouldContain("2");
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

        await EffectAsync(() => DoWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        effectResult.ShouldBeTrue();
        return;

        Task DoWork() => Effect(() => effectResult = true).Async();
    }
}