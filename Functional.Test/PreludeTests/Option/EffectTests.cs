namespace Functional.Test.PreludeTests.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class EffectTests
{
    [TestMethod]
    public async Task OptionShouldPerformEffectsAsync()
    {
        var msg = "";

        void someEffectWithInput(string input) => msg = input;
        void someEffectWithoutInput() => msg = "Some";
        Task someEffectWithInputAsTask(string input) => EffectAsync(() => msg = input);
        Task someEffectWithoutInputAsTask() => EffectAsync(() => msg = "Some");
        void noneEffect() => msg = "None";
        Task noneEffectAsTask() => EffectAsync(() => msg = "None");
        void reset() => msg = string.Empty;

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithInput,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithInput,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithInput,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithInput,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithoutInput,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithoutInput,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithoutInput,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithoutInput,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithInputAsTask,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithInputAsTask,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithInputAsTask,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithInputAsTask,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithoutInputAsTask,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithoutInputAsTask,
                noneEffect)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectAsync(
                someEffectWithoutInputAsTask,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectAsync(
                someEffectWithoutInputAsTask,
                noneEffectAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldPerformEffectsWhenSome()
    {
        var msg = "";

        void someEffectWithInput(string input) => msg = input;
        void someEffectWithoutInput() => msg = "Some";
        Task someEffectWithInputAsTask(string input) => EffectAsync(() => msg = input);
        Task someEffectWithoutInputAsTask() => EffectAsync(() => msg = "Some");
        void reset() => msg = string.Empty;

        await Some("123")
            .Async()
            .EffectSomeAsync(someEffectWithInput)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectSomeAsync(someEffectWithInput)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);

        await Some("123")
            .Async()
            .EffectSomeAsync(someEffectWithoutInput)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectSomeAsync(someEffectWithoutInput)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);

        await Some("123")
            .Async()
            .EffectSomeAsync(someEffectWithInputAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("123");
        reset();

        await None<string>()
            .Async()
            .EffectSomeAsync(someEffectWithInputAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);

        await Some("123")
            .Async()
            .EffectSomeAsync(someEffectWithoutInputAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("Some");
        reset();

        await None<string>()
            .Async()
            .EffectSomeAsync(someEffectWithoutInputAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldPerformEffectsWhenNone()
    {
        var msg = string.Empty;

        void noneAction() => msg = "None";
        Task noneActionAsTask() => EffectAsync(() => msg = "None");
        void reset() => msg = string.Empty;

        await Some("123")
            .Async()
            .EffectNoneAsync(noneAction)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);

        await None<string>()
            .Async()
            .EffectNoneAsync(noneAction)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
        reset();

        await Some("123")
            .Async()
            .EffectNoneAsync(noneActionAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe(string.Empty);

        await None<string>()
            .Async()
            .EffectNoneAsync(noneActionAsTask)
            .TapAsync(output => output.ShouldBeOfType<Unit>());

        msg.ShouldBe("None");
    }
}
