namespace Functional.Test.Options;

[TestClass]
[ExcludeFromCodeCoverage]
public class OptionTests
{
    [TestMethod]
    public void NoneShouldBeOfOptionType() =>
        (null as string)
            .Optional()
            .ShouldBeOfType<Option<string>>();

    [TestMethod]
    public void SomeShouldBeOfOptionType() =>
        1
            .Optional()
            .ShouldBeOfType<Option<int>>();

    [TestMethod]
    public void FilterShouldFindSomeWhenTrue() =>
        "find me".Optional()
            .Filter(value => value == "find me")
            .Match(some => some, () => "not found")
            .ShouldBe("find me");

    [TestMethod]
    public void FilterShouldNotFindSomeWhenFalse() =>
        "cant find me".Optional()
            .Filter(value => value == "find me")
            .Match(some => some, () => "not found")
            .ShouldBe("not found");

    [TestMethod]
    public void FilterShouldNotFindNone() =>
        (null as string)
            .Optional()
            .Filter(value => value == "find me")
            .Match(value => value, () => "not found")
            .ShouldBe("not found");

    [TestMethod]
    public async Task FilterShouldFindSomeWhenTrueAsync() =>
        await "find  me"
            .Optional()
            .Async()
            .FilterAsync(value => value == "find  me")
            .MatchAsync(some => some, () => "not found")
            .TapAsync(value => value.ShouldBe("find  me"));

    [TestMethod]
    public async Task FilterShouldNotFindSomeWhenFalseAsync() =>
        await "cant find me"
            .Optional()
            .Async()
            .FilterAsync(value => value == "find me")
            .MatchAsync(some => some, () => "not found")
            .TapAsync(value => value.ShouldBe("not found"));

    [TestMethod]
    public async Task FilterShouldNotFindNoneAsync() =>
        await None<string>()
            .Async()
            .FilterAsync(value => value == "find me")
            .MatchAsync(value => value, () => "not found")
            .TapAsync(value => value.ShouldBe("not found"));

    [TestMethod]
    public void ReduceShouldUseContentWhenSome() =>
        Some("contents")
            .Reduce("other")
            .ShouldBe("contents");

    [TestMethod]
    public void ReduceShouldUseContentWhenSomeWithDelegate() =>
        Some("contents")
            .Reduce(() => "other")
            .ShouldBe("contents");

    [TestMethod]
    public void ReduceShouldUseAlternateValueWhenNone() =>
        None<string>()
            .Reduce("other")
            .ShouldBe("other");

    [TestMethod]
    public void ReduceShouldUseDelegateValueWhenNone() =>
        None<string>()
            .Reduce(() => "other delegate")
            .ShouldBe("other delegate");

    [TestMethod]
    public async Task ItShouldMapSomeAsync() =>
        await 1
            .Optional()
            .Async()
            .MapAsync(val => val.ToString())
            .ReduceAsync("something else")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMapNoneAsync() =>
        await None<int>()
            .Async()
            .MapAsync(val => val.ToString())
            .ReduceAsync("something else")
            .TapAsync(str => str.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldReduceSomeAsyncWithFunctions() =>
        await 1
            .Optional()
            .Async()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldReduceNoneAsyncWithFunctions() =>
        await None<int>()
            .Async()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldMapAsyncWithTaskOfTask() =>
        await 1.Async()
            .Optional()
            .MapAsync(one => one.ToString().Async())
            .MapAsync(result => (result + "2").Async())
            .ReduceAsync("something else")
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldMapAndReduceFuncAsyncWithTaskOfTask() =>
        await 1.Async()
            .Optional()
            .MapAsync(one => one.ToString().Async())
            .MapAsync(result => (result + "2").Async())
            .ReduceAsync(() => "something else".Async())
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldMapAndReduceAsyncWithTaskOfTask() =>
        await 1.Async()
            .Optional()
            .MapAsync(one => one.ToString().Async())
            .MapAsync(result => (result + "2").Async())
            .ReduceAsync("something else".Async())
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldReduceAsyncWithTaskOfTaskAndAsyncReducer() =>
        await None<int>()
            .Async()
            .MapAsync(one => one.ToString().Async())
            .MapAsync(result => (result + "2").Async())
            .ReduceAsync("something else".Async())
            .TapAsync(final => final.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldReduceAsyncWithTaskOfTaskAndFuncAsyncReducer() =>
        await None<int>()
            .Async()
            .MapAsync(one => one.ToString().Async())
            .MapAsync(result => (result + "2").Async())
            .ReduceAsync(() => "something else".Async())
            .TapAsync(final => final.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldHandleAsyncNoneOptions() =>
        await (null as string)
            .Async()
            .Optional()
            .TapAsync(result => result.ShouldBeEquivalentTo(None<string>()));

    [TestMethod]
    public async Task ItShouldHandleAsyncSomeOptions() =>
        await AlwaysReturnsNullableString("input")
            .Optional()
            .TapAsync(result => result.ShouldBeEquivalentTo(Some("input")));

    [TestMethod]
    public async Task ItShouldMapOptionOfTaskOfString() =>
        await Some("input")
            .Map(input => input.Async())
            .MapAsync(value => value + "!")
            .ReduceAsync("Oops.")
            .TapAsync(result => result.ShouldBe("input!"));

    [TestMethod]
    public async Task ItShouldMayOptionOfTaskOfStringWhenNone() =>
        await None<string>()
            .Map(input => input.Async())
            .MapAsync(value => value + "!")
            .ReduceAsync("Oops.")
            .TapAsync(result => result.ShouldBe("Oops."));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithValue() =>
    await None<Task<string>>()
        .ReduceAsync("Oops")
        .TapAsync(result => result.ShouldBe("Oops"));


    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithAsyncValue() =>
    await None<Task<string>>()
        .ReduceAsync("Oops".Async())
        .TapAsync(result => result.ShouldBe("Oops"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithRegularFunction() =>
    await None<Task<string>>()
        .ReduceAsync(() => "Oops")
        .TapAsync(result => result.ShouldBe("Oops"));


    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithAsyncFunction() =>
    await None<Task<string>>()
        .ReduceAsync(() => "Oops".Async())
        .TapAsync(result => result.ShouldBe("Oops"));

    private static async Task<string?> AlwaysReturnsNullableString(string input) =>
        await input.Async();

    [TestMethod]
    public async Task ItShouldMatchSomeAsync() =>
        await Some(1)
            .Async()
            .MatchAsync(
                some => some.ToString(),
                () => "none")
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchNoneAsync() =>
        await None<int>()
            .Async()
            .MatchAsync(
                some => some.ToString(),
                () => "none")
            .TapAsync(res => res.ShouldBe("none"));

    [TestMethod]
    public void ItShouldBindOptionsWhenSome() =>
        Some(1)
            .Bind(one => one.ToString().Optional())
            .ShouldBeEquivalentTo(Some("1"));

    [TestMethod]
    public void ItShouldBindOptionsWhenBecomeNone() =>
        Some(1)
            .Bind(one => None<string>())
            .ShouldBeEquivalentTo(None<string>());

    [TestMethod]
    public void ItShouldBindOptionsWhenStartAsNone() =>
        None<int>()
            .Bind(one => one.ToString().Optional())
            .ShouldBeEquivalentTo(None<string>());

    [TestMethod]
    public async Task ItShouldBindAsyncWhenSome() =>
        await Some(1)
            .Async()
            .BindAsync(one => one.ToString().Optional())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Some("1")));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenStartAsNone() =>
        await None<int>()
            .Async()
            .BindAsync(one => one.ToString().Optional())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenBecomesNone() =>
        await Some(1)
            .Async()
            .BindAsync(_ => None<string>())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenSomeWithTaskBinding() =>
    await Some(1)
        .Async()
        .BindAsync(one => one.ToString().Optional().Async())
        .TapAsync(bound => bound.ShouldBeEquivalentTo(Some("1")));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenStartAsNoneWithTaskBinding() =>
        await None<int>()
            .Async()
            .BindAsync(one => one.ToString().Optional().Async())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenBecomesNoneWithTaskBinding() =>
        await Some(1)
            .Async()
            .BindAsync(_ => None<string>().Async())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(None<string>()));

    [TestMethod]
    public void ItShouldBeSome() =>
        Some("some value")
            .IsSome
            .ShouldBeTrue();

    [TestMethod]
    public void ItShouldNotBeNone() =>
        Some("some value")
            .IsNone
            .ShouldBeFalse();

    [TestMethod]
    public void ItShouldBeNone() =>
        None<string>()
            .IsNone
            .ShouldBeTrue();

    [TestMethod]
    public void ItShouldNotBeSome() =>
        None<string>()
            .IsSome
            .ShouldBeFalse();

    [TestMethod]
    public void ItShouldDoEffectsWhenSome()
    {
        var wasCalled = false;
        void whenSome(string _) => wasCalled = true;
        void whenNone() => wasCalled = false;

        _ = Some("value")
            .Tap(someValue => someValue.Effect(whenSome, whenNone));

        wasCalled.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldDoEffectsWhenNone()
    {
        var wasCalled = false;
        void whenSome(string _) => wasCalled = false;
        void whenNone() => wasCalled = true;

        None<string>()
            .Effect(whenSome, whenNone);

        wasCalled.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldUnwrapSomeValue() =>
        Some("some value")
        .Unwrap()
        .ShouldBe("some value");

    [TestMethod]
    public void ItShouldThrowWhenUnwrappingNone()
    {
        static void callback() => None<int>().Unwrap();
        Assert.ThrowsException<InvalidOperationException>(callback);
    }

    [TestMethod]
    public async Task ItShouldThrowWhenUnwrappingNoneAsync()
    {
        static Task callback() => None<int>().Async().UnwrapAsync();
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(callback);
    }

    [TestMethod]
    public async Task ItShouldSuccessfullyUnwrapSomeAsync() =>
        await Some(1)
            .Async()
            .UnwrapAsync()
            .TapAsync(value => value.ShouldBe(1))
            .IgnoreAsync();

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenSome()
    {
        var msg = "";

        await Some("123")
            .Async()
            .EffectAsync(
                some => msg = some,
                () => msg = "None");

        msg.ShouldBe("123");
    }

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenNone()
    {
        var msg = "";

        await None<string>()
            .Async()
            .EffectAsync(
                some => msg = some,
                () => msg = "None");

        msg.ShouldBe("None");
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenSome()
    {
        var taskOfTask =
            "something"
                .Optional()
                .Async()
                .MatchAsync(
                    some => some.Async(),
                    () => "none".Async());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone()
    {
        var taskOfTask =
            None<string>()
                .Async()
                .MatchAsync(
                    some => some.Async(),
                    () => "none".Async());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenSome1()
    {
        var taskOfTask =
            "something"
                .Optional()
                .Async()
                .MatchAsync(
                    some => some.Async(),
                    () => "none");

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone1()
    {
        var taskOfTask =
            None<string>()
                .Async()
                .MatchAsync(
                    some => some.Async(),
                    () => "none");

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenSome2()
    {
        var taskOfTask =
            "something"
                .Optional()
                .Async()
                .MatchAsync(
                    some => some,
                    () => "none".Async());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone2()
    {
        var taskOfTask =
            None<string>()
                .Async()
                .MatchAsync(
                    some => some,
                    () => "none".Async());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenSome3()
    {
        var taskOfTask =
            "something"
                .Optional()
                .Async()
                .MatchAsync(
                    some => some,
                    () => "none");

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone3()
    {
        var taskOfTask =
            None<string>()
                .Async()
                .MatchAsync(
                    some => some,
                    () => "none");

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldHandleValueTasks()
    {
        var option =
            ValueTask
                .FromResult(1 as int?)
                .Optional();

        (await option)
                .ShouldBeOfType<Option<int>>()
                .Ignore();
    }

    [TestMethod]
    public async Task ItShouldHandleClassValueTasks()
    {
        var option =
            ValueTask
                .FromResult(null as string)
                .Optional();

        (await option)
            .ShouldBeOfType<Option<string>>()
            .Ignore();
    }

    [TestMethod]
    public void ItShouldHandleStructsWhenNull() =>
        (null as int?)
        .Optional()
        .Map(thing => thing.ToString())
        .Reduce(() => "none")
        .ShouldBe("none");

    [TestMethod]
    public void ItShouldMapOptionsAndIgnoreInputs() =>
        Some("arbitrary value")
            .Map(() => "new value")
            .EffectSome(value => value.ShouldBe("new value"))
            .ShouldBeOfType<Unit>();

    [TestMethod]
    public void ItShouldHandleEffectsForOptionsWhenSome()
    {
        var someResult = false;
        var noneResult = false;

        Some("value")
            .Effect(
                () => someResult = true,
                () => noneResult = true);

        someResult.ShouldBeTrue();
        noneResult.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldHandleEffectSome()
    {
        var someResult = false;
        var noneResult = false;

        Some("value")
            .EffectSome(value => someResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeTrue();
        noneResult.ShouldBeFalse();

        someResult = false;
        noneResult = false;

        None<string>()
            .EffectSome(value => someResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeFalse();
        noneResult.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldHandleEffectNone()
    {
        var someResult = false;
        var noneResult = false;

        Some("value")
            .EffectNone(() => noneResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeFalse();
        noneResult.ShouldBeFalse();

        None<string>()
            .EffectNone(() => noneResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeFalse();
        noneResult.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldHandleEffectsForOptionsWhenNone()
    {
        var someResult = false;
        var noneResult = false;

        None<string>()
            .Effect(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeFalse();
        noneResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithActionsWhenSome()
    {
        var someResult = false;
        var noneResult = false;

        await Some("value")
            .Async()
            .EffectAsync(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeTrue();
        noneResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithActionsWhenNone()
    {
        var someResult = false;
        var noneResult = false;

        await None<string>()
            .Async()
            .EffectAsync(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeFalse();
        noneResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenSomeWithInput()
    {
        var someResult = string.Empty;

        await Some("value")
            .Async()
            .EffectSomeAsync(value => someResult = value);

        someResult.ShouldBe("value");
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenNoneWithInput()
    {
        var someResult = string.Empty;

        await None<string>()
            .Async()
            .EffectSomeAsync(value => someResult = value);

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenSomeNoInput()
    {
        var someResult = string.Empty;

        await Some("value")
            .Async()
            .EffectSomeAsync(() => someResult = "something");

        someResult.ShouldBe("something");
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenNoneNoInput()
    {
        var someResult = string.Empty;

        await None<string>()
            .Async()
            .EffectSomeAsync(() => someResult = "something");

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeAsyncWhenUsingFuncT()
    {
        var someResult = string.Empty;

        Task doWork(string input) => Effect(() => someResult = input).Pipe(Task.CompletedTask);

        await Some("value")
            .Async()
            .EffectSomeAsync(value => doWork(value))
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        someResult.ShouldBe("value");
        someResult = string.Empty;

        await None<string>()
            .Async()
            .EffectSomeAsync(value => doWork(value))
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeAsyncWhenUsingFuncTask()
    {
        var someResult = string.Empty;

        Task doWork() => Effect(() => someResult = "doWork called").Pipe(Task.CompletedTask);

        await Some("value")
            .Async()
            .EffectSomeAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        someResult.ShouldBe("doWork called");
        someResult = string.Empty;

        await None<string>()
            .Async()
            .EffectSomeAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectNoneWhenSome()
    {
        var noneResult = string.Empty;

        await Some("value")
            .Async()
            .EffectNoneAsync(() => noneResult = "none");

        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectNoneWhenNone()
    {
        var noneResult = string.Empty;

        await None<string>()
            .Async()
            .EffectNoneAsync(() => noneResult = "none");

        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldHandleEffectNoneAsyncForFuncTask()
    {
        var noneResult = string.Empty;

        Task doWork() => Effect(() => noneResult = "none").Pipe(Task.CompletedTask);

        await None<string>()
            .Async()
            .EffectNoneAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        noneResult.ShouldBe("none");

        noneResult = string.Empty;

        await Some("value")
            .Async()
            .EffectNoneAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public void ItShouldTapOptions()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        None<string>()
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public void ItShouldTapOptionTypes()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        None<string>()
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public void ItShouldTapOptionTypesWithPlainAction()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .Tap(() => someResult = "some", () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        None<string>()
            .Tap(() => someResult = "some", () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public void ItShouldTapSomeWithValue()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .TapSome(value => someResult = value)
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        None<string>()
            .TapSome(value => someResult = value)
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public void ItShouldTapSomeWithNoValue()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .TapSome(() => someResult = "some")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        None<string>()
            .TapSome(() => someResult = "some")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public void ItShouldTapNone()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Some("value")
            .TapNone(() => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        None<string>()
            .TapNone(() => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeActionTNoneAction()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        void doSomeWork(string input) => someResult = input;
        void doNoneWork() => noneResult = "none";

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeActionTNoneFuncTask()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        void doSomeWork(string input) => someResult = input;
        Task doNoneWork() => EffectAsync(() => noneResult = "none");

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeActionNoneAction()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        void doSomeWork() => someResult = "some";
        void doNoneWork() => noneResult = "none";

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeActionNoneFuncTask()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        void doSomeWork() => someResult = "some";
        Task doNoneWork() => EffectAsync(() => noneResult = "none");

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeFuncTTaskNoneAction()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Task doSomeWork(string input) => EffectAsync(() => someResult = input);
        void doNoneWork() => noneResult = "none";

        await Some("some")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeFuncTTaskNoneFuncTask()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Task doSomeWork(string input) => EffectAsync(() => someResult = input);
        Task doNoneWork() => EffectAsync(() => noneResult = "none");

        await Some("some")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeFuncTaskNoneAction()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Task doSomeWork() => EffectAsync(() => someResult = "some");
        void doNoneWork() => noneResult = "none";

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncSomeFuncTaskNoneFuncTask()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Task doSomeWork() => EffectAsync(() => someResult = "some");
        Task doNoneWork() => EffectAsync(() => noneResult = "none");

        await Some("value")
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapAsync(doSomeWork, doNoneWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapSomeAsyncActionT()
    {
        var someResult = string.Empty;

        void doSomeWork(string input) => someResult = input;


        await Some("some")
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapSomeAsyncAction()
    {
        var someResult = string.Empty;

        void doSomeWork() => someResult = "some";


        await Some("value")
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapSomeAsyncFuncTTask()
    {
        var someResult = string.Empty;

        Task doSomeWork(string input) => EffectAsync(() => someResult = input);


        await Some("some")
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapSomeAsyncFuncTask()
    {
        var someResult = string.Empty;

        Task doSomeWork() => EffectAsync(() => someResult = "some");


        await Some("value")
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe("some");

        someResult = string.Empty;

        await None<string>()
            .Async()
            .TapSomeAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapNoneAsyncAction()
    {
        var noneResult = string.Empty;

        void doSomeWork() => noneResult = "none";


        await Some("value")
            .Async()
            .TapNoneAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        noneResult.ShouldBe(string.Empty);

        noneResult = string.Empty;

        await None<string>()
            .Async()
            .TapNoneAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public async Task ItShouldTapNoneAsyncFuncTask()
    {
        var noneResult = string.Empty;

        Task doSomeWork() => EffectAsync(() => noneResult = "none");


        await Some("value")
            .Async()
            .TapNoneAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        noneResult.ShouldBe(string.Empty);

        noneResult = string.Empty;

        await None<string>()
            .Async()
            .TapNoneAsync(doSomeWork)
            .TapAsync(output => output.ShouldBeOfType<Option<string>>())
            .IgnoreAsync();

        noneResult.ShouldBe("none");
    }
}