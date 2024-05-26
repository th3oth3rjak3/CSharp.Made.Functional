namespace Functional.Test.Prelude;

[ExcludeFromCodeCoverage]
[TestClass]
public class TapTests
{
    private readonly List<string> effects = [];

    [TestInitialize]
    public void Reset() => effects.Clear();

    [TestMethod]
    public void ItShouldTapInputs_1()
    {
        "input"
            .Tap(
                i => effects.Add(i),
                i => effects.Add(i + "!"))
            .AssertInstanceOfType(typeof(string))
            .ShouldBe("input");

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("input");
        effects[1].ShouldBe("input!");
    }
    
    [TestMethod]
    public void ItShouldTapInputs_2()
    {
        "input"
            .Tap(
                () => effects.Add("effect"),
                () => effects.Add("effect!"))
            .AssertInstanceOfType(typeof(string))
            .ShouldBe("input");

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("effect");
        effects[1].ShouldBe("effect!");
    }
    
    [TestMethod]
    public async Task ItShouldTapInputsAsync_1()
    {
        await "input"
            .Async()
            .TapAsync(
                i => effects.Add(i),
                i => effects.Add(i + "!"))
            .AssertInstanceOfType(typeof(Task<string>))
            .EffectAsync(output => output.ShouldBe("input"));

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("input");
        effects[1].ShouldBe("input!");
    }
    
    [TestMethod]
    public async Task ItShouldTapInputsAsync_2()
    {
        await "input"
            .Async()
            .TapAsync(
                () => effects.Add("effect"),
                () => effects.Add("effect!"))
            .AssertInstanceOfType(typeof(Task<string>))
            .EffectAsync(output => output.ShouldBe("input"));

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("effect");
        effects[1].ShouldBe("effect!");
    }
    
    [TestMethod]
    public async Task ItShouldTapInputsAsync_3()
    {
        await "input"
            .Async()
            .TapAsync(
                i => EffectAsync(() => effects.Add(i)),
                i => EffectAsync(() => effects.Add(i + "!")))
            .AssertInstanceOfType(typeof(Task<string>))
            .EffectAsync(output => output.ShouldBe("input"));

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("input");
        effects[1].ShouldBe("input!");
    }
    
    [TestMethod]
    public async Task ItShouldTapInputsAsync_4()
    {
        await "input"
            .Async()
            .TapAsync(
                () => EffectAsync(() => effects.Add("effect")),
                () => EffectAsync(() => effects.Add("effect!")))
            .AssertInstanceOfType(typeof(Task<string>))
            .EffectAsync(output => output.ShouldBe("input"));

        effects.Count.ShouldBe(2);
        effects[0].ShouldBe("effect");
        effects[1].ShouldBe("effect!");
    }
}