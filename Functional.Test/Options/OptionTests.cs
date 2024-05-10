
using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Options;

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
        "find me".Some()
            .Filter(value => value == "find me")
            .Match(some => some, () => "not found")
            .ShouldBe("find me");

    [TestMethod]
    public void FilterShouldNotFindSomeWhenFalse() =>
        "cant find me".Some()
            .Filter(value => value == "find me")
            .Match(some => some, () => "not found")
            .ShouldBe("not found");

    [TestMethod]
    public void FilterShouldNotFindNone() =>
        Option.None<string>()
            .Filter(value => value == "find me")
            .Match(value => value, () => "not found")
            .ShouldBe("not found");

    [TestMethod]
    public async Task FilterShouldFindSomeWhenTrueAsync() =>
        await "find  me"
            .Some()
            .AsAsync()
            .FilterAsync(value => value == "find  me")
            .MatchAsync(some => some, () => "not found")
            .TapAsync(value => value.ShouldBe("find  me"));

    [TestMethod]
    public async Task FilterShouldNotFindSomeWhenFalseAsync() =>
        await "cant find me"
            .Some()
            .AsAsync()
            .FilterAsync(value => value == "find me")
            .MatchAsync(some => some, () => "not found")
            .TapAsync(value => value.ShouldBe("not found"));

    [TestMethod]
    public async Task FilterShouldNotFindNoneAsync() =>
        await Option.None<string>()
            .AsAsync()
            .FilterAsync(value => value == "find me")
            .MatchAsync(value => value, () => "not found")
            .TapAsync(value => value.ShouldBe("not found"));

    [TestMethod]
    public void ReduceShouldUseContentWhenSome() =>
        "contents".Some()
            .Reduce("other")
            .ShouldBe("contents");

    [TestMethod]
    public void ReduceShouldUseContentWhenSomeWithDelegate() =>
        "contents".Some()
            .Reduce(() => "other")
            .ShouldBe("contents");

    [TestMethod]
    public void ReduceShouldUseAlternateValueWhenNone() =>
        Option.None<string>()
            .Reduce("other")
            .ShouldBe("other");

    [TestMethod]
    public void ReduceShouldUseDelegateValueWhenNone() =>
        Option.None<string>()
            .Reduce(() => "other delegate")
            .ShouldBe("other delegate");

    [TestMethod]
    public async Task ItShouldMapSomeAsync() =>
        await 1
            .Optional()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync("something else")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMapNoneAsync() =>
        await Option.None<int>()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync("something else")
            .TapAsync(str => str.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldReduceSomeAsyncWithFunctions() =>
        await 1
            .Optional()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldReduceNoneAsyncWithFunctions() =>
        await Option.None<int>()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldMapAsyncWithTaskOfTask() =>
        await 1.AsAsync()
            .Optional()
            .MapAsync(one => one.ToString().AsAsync())
            .MapAsync(result => (result + "2").AsAsync())
            .ReduceAsync("something else")
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldMapAndReduceFuncAsyncWithTaskOfTask() =>
        await 1.AsAsync()
            .Optional()
            .MapAsync(one => one.ToString().AsAsync())
            .MapAsync(result => (result + "2").AsAsync())
            .ReduceAsync(() => "something else".AsAsync())
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldMapAndReduceAsyncWithTaskOfTask() =>
        await 1.AsAsync()
            .Optional()
            .MapAsync(one => one.ToString().AsAsync())
            .MapAsync(result => (result + "2").AsAsync())
            .ReduceAsync("something else".AsAsync())
            .TapAsync(final => final.ShouldBe("12"));

    [TestMethod]
    public async Task ItShouldReduceAsyncWithTaskOfTaskAndAsyncReducer() =>
        await Option.None<int>()
            .AsAsync()
            .MapAsync(one => one.ToString().AsAsync())
            .MapAsync(result => (result + "2").AsAsync())
            .ReduceAsync("something else".AsAsync())
            .TapAsync(final => final.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldReduceAsyncWithTaskOfTaskAndFuncAsyncReducer() =>
        await Option.None<int>()
            .AsAsync()
            .MapAsync(one => one.ToString().AsAsync())
            .MapAsync(result => (result + "2").AsAsync())
            .ReduceAsync(() => "something else".AsAsync())
            .TapAsync(final => final.ShouldBe("something else"));

    [TestMethod]
    public async Task ItShouldHandleAsyncNoneOptions() =>
        await (null as string)
            .AsAsync()
            .Optional()
            .TapAsync(result => result.ShouldBeEquivalentTo(Option.None<string>()));

    [TestMethod]
    public async Task ItShouldHandleAsyncSomeOptions() =>
        await AlwaysReturnsNullableString("input")
            .Optional()
            .TapAsync(result => result.ShouldBeEquivalentTo("input".Some()));

    [TestMethod]
    public async Task ItShouldMapOptionOfTaskOfString() =>
        await "input".Some()
            .Map(input => input.AsAsync())
            .MapAsync(value => value + "!")
            .ReduceAsync("Oops.")
            .TapAsync(result => result.ShouldBe("input!"));

    [TestMethod]
    public async Task ItShouldMayOptionOfTaskOfStringWhenNone() =>
        await Option.None<string>()
            .Map(input => input.AsAsync())
            .MapAsync(value => value + "!")
            .ReduceAsync("Oops.")
            .TapAsync(result => result.ShouldBe("Oops."));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenSomeWithValue() =>
        await "input"
            .AsAsync()
            .Pipe(Option.Some)
            .ReduceAsync("Oops")
            .TapAsync(result => result.ShouldBe("input"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithValue() =>
    await Option.None<Task<string>>()
        .ReduceAsync("Oops")
        .TapAsync(result => result.ShouldBe("Oops"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenSomeWithAsyncValue() =>
    await "input"
        .AsAsync()
        .Pipe(Option.Some)
        .ReduceAsync("Oops".AsAsync())
        .TapAsync(result => result.ShouldBe("input"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithAsyncValue() =>
    await Option.None<Task<string>>()
        .ReduceAsync("Oops".AsAsync())
        .TapAsync(result => result.ShouldBe("Oops"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenSomeWithRegularFunction() =>
    await "input"
        .AsAsync()
        .Pipe(Option.Some)
        .ReduceAsync(() => "Oops")
        .TapAsync(result => result.ShouldBe("input"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithRegularFunction() =>
    await Option.None<Task<string>>()
        .ReduceAsync(() => "Oops")
        .TapAsync(result => result.ShouldBe("Oops"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenSomeWithAsyncFunction() =>
    await "input"
        .AsAsync()
        .Pipe(Option.Some)
        .ReduceAsync(() => "Oops".AsAsync())
        .TapAsync(result => result.ShouldBe("input"));

    [TestMethod]
    public async Task ItShouldReduceOptionOfTaskOfStringWhenNoneWithAsyncFunction() =>
    await Option.None<Task<string>>()
        .ReduceAsync(() => "Oops".AsAsync())
        .TapAsync(result => result.ShouldBe("Oops"));

    private static async Task<string?> AlwaysReturnsNullableString(string input) =>
        await input.AsAsync();

    [TestMethod]
    public async Task ItShouldMatchSomeAsync() =>
        await Option.Some(1)
            .AsAsync()
            .MatchAsync(
                some => some.ToString(),
                () => "none")
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchNoneAsync() =>
        await Option
            .None<int>()
            .AsAsync()
            .MatchAsync(
                some => some.ToString(),
                () => "none")
            .TapAsync(res => res.ShouldBe("none"));

    [TestMethod]
    public void ItShouldBindOptionsWhenSome() =>
        Option.Some(1)
            .Bind(one => one.ToString().Optional())
            .ShouldBeEquivalentTo(Option.Some("1"));

    [TestMethod]
    public void ItShouldBindOptionsWhenBecomeNone() =>
        Option.Some(1)
            .Bind(one => Option.None<string>())
            .ShouldBeEquivalentTo(Option.None<string>());

    [TestMethod]
    public void ItShouldBindOptionsWhenStartAsNone() =>
        Option.None<int>()
            .Bind(one => one.ToString().Optional())
            .ShouldBeEquivalentTo(Option.None<string>());

    [TestMethod]
    public async Task ItShouldBindAsyncWhenSome() =>
        await Option.Some(1)
            .AsAsync()
            .BindAsync(one => one.ToString().Optional())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.Some("1")));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenStartAsNone() =>
        await Option.None<int>()
            .AsAsync()
            .BindAsync(one => one.ToString().Optional())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenBecomesNone() =>
        await Option.Some(1)
            .AsAsync()
            .BindAsync(_ => Option.None<string>())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenSomeWithTaskBinding() =>
    await Option.Some(1)
        .AsAsync()
        .BindAsync(one => one.ToString().Optional().AsAsync())
        .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.Some("1")));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenStartAsNoneWithTaskBinding() =>
        await Option.None<int>()
            .AsAsync()
            .BindAsync(one => one.ToString().Optional().AsAsync())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.None<string>()));

    [TestMethod]
    public async Task ItShouldBindAsyncWhenBecomesNoneWithTaskBinding() =>
        await Option.Some(1)
            .AsAsync()
            .BindAsync(_ => Option.None<string>().AsAsync())
            .TapAsync(bound => bound.ShouldBeEquivalentTo(Option.None<string>()));

    [TestMethod]
    public void ItShouldBeSome() =>
        "some value"
            .Some()
            .IsSome
            .ShouldBeTrue();

    [TestMethod]
    public void ItShouldNotBeNone() =>
        "some value"
            .Some()
            .IsNone
            .ShouldBeFalse();

    [TestMethod]
    public void ItShouldBeNone() =>
        Option.None<string>()
            .IsNone
            .ShouldBeTrue();

    [TestMethod]
    public void ItShouldNotBeSome() =>
        Option.None<string>()
            .IsSome
            .ShouldBeFalse();

    [TestMethod]
    public void ItShouldDoEffectsWhenSome()
    {
        var wasCalled = false;
        void whenSome(string _) => wasCalled = true;
        void whenNone() => wasCalled = false;

        _ = "value"
            .Some()
            .Tap(someValue => someValue.Effect(whenSome, whenNone));

        wasCalled.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldDoEffectsWhenNone()
    {
        var wasCalled = false;
        void whenSome(string _) => wasCalled = false;
        void whenNone() => wasCalled = true;

        Option.None<string>()
            .Effect(whenSome, whenNone);

        wasCalled.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldUnwrapSomeValue() =>
    "some value"
        .Some()
        .Unwrap()
        .ShouldBe("some value");

    [TestMethod]
    public void ItShouldBeNullWhenUnwrappingNone() =>
        Option.None<string>()
            .Unwrap()
            .ShouldBeNull();

    [TestMethod]
    public void ItShouldBeDefaultWhenUnwrappingIntegers() =>
        Option.None<int>()
            .Unwrap()
            .ShouldBe(0);

    [TestMethod]
    public async Task ItShouldBeNullWhenUnwrappingNoneAsync() =>
        await Option.None<string>()
            .AsAsync()
            .UnwrapAsync()
            .TapAsync(value => value.ShouldBeNull());

    [TestMethod]
    public async Task ItShouldBeDefaultWhenUnwrappingIntegersAsync() =>
        await Option.None<int>()
            .AsAsync()
            .UnwrapAsync()
            .TapAsync(value => value.ShouldBe(0));

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenSome()
    {
        var msg = "";

        await "123"
            .Some()
            .AsAsync()
            .EffectAsync(
                some => msg = some,
                () => msg = "None");

        msg.ShouldBe("123");
    }

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenNone()
    {
        var msg = "";

        await Option.None<string>()
            .AsAsync()
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
                .AsAsync()
                .MatchAsync(
                    some => some.AsAsync(),
                    () => "none".AsAsync());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone()
    {
        var taskOfTask =
            Option.None<string>()
                .AsAsync()
                .MatchAsync(
                    some => some.AsAsync(),
                    () => "none".AsAsync());

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
                .AsAsync()
                .MatchAsync(
                    some => some.AsAsync(),
                    () => "none");

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone1()
    {
        var taskOfTask =
            Option.None<string>()
                .AsAsync()
                .MatchAsync(
                    some => some.AsAsync(),
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
                .AsAsync()
                .MatchAsync(
                    some => some,
                    () => "none".AsAsync());

        (await taskOfTask)
            .ShouldBeOfType<string>()
            .Ignore();
    }

    [TestMethod]
    public async Task ItShouldNotReturnTaskOfTaskWhenNone2()
    {
        var taskOfTask =
            Option.None<string>()
                .AsAsync()
                .MatchAsync(
                    some => some,
                    () => "none".AsAsync());

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
                .AsAsync()
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
            Option.None<string>()
                .AsAsync()
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
        Option.Some("arbitrary value")
            .Map(() => "new value")
            .EffectSome(value => value.ShouldBe("new value"))
            .ShouldBeOfType<Unit>();

    [TestMethod]
    public void ItShouldHandleEffectsForOptionsWhenSome()
    {
        var someResult = false;
        var noneResult = false;

        Option.Some("value")
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

        Option.Some("value")
            .EffectSome(value => someResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeTrue();
        noneResult.ShouldBeFalse();

        someResult = false;
        noneResult = false;

        Option.None<string>()
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

        Option.Some("value")
            .EffectNone(() => noneResult = true)
            .ShouldBeOfType<Unit>();

        someResult.ShouldBeFalse();
        noneResult.ShouldBeFalse();

        Option.None<string>()
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

        Option.None<string>()
            .Effect(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeFalse();
        noneResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithActionsWhenSome()
    {
        var someResult = false;
        var noneResult = false;

        await Option.Some("value")
            .AsAsync()
            .EffectAsync(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeTrue();
        noneResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithActionsWhenNone()
    {
        var someResult = false;
        var noneResult = false;

        await Option.None<string>()
            .AsAsync()
            .EffectAsync(() => someResult = true, () => noneResult = true);

        someResult.ShouldBeFalse();
        noneResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenSomeWithInput()
    {
        var someResult = string.Empty;

        await Option.Some("value")
            .AsAsync()
            .EffectSomeAsync(value => someResult = value);

        someResult.ShouldBe("value");
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenNoneWithInput()
    {
        var someResult = string.Empty;

        await Option.None<string>()
            .AsAsync()
            .EffectSomeAsync(value => someResult = value);

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenSomeNoInput()
    {
        var someResult = string.Empty;

        await Option.Some("value")
            .AsAsync()
            .EffectSomeAsync(() => someResult = "something");

        someResult.ShouldBe("something");
    }

    [TestMethod]
    public async Task ItShouldHandleEffectSomeWhenNoneNoInput()
    {
        var someResult = string.Empty;

        await Option.None<string>()
            .AsAsync()
            .EffectSomeAsync(() => someResult = "something");

        someResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectNoneWhenSome()
    {
        var noneResult = string.Empty;

        await Option.Some("value")
            .AsAsync()
            .EffectNoneAsync(() => noneResult = "none");

        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldHandleEffectNoneWhenNone()
    {
        var noneResult = string.Empty;

        await Option.None<string>()
            .AsAsync()
            .EffectNoneAsync(() => noneResult = "none");

        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public void ItShouldTapOptions()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        Option.Some("value")
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        Option.None<string>()
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

        Option.Some("value")
            .Tap(value => someResult = value, () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        Option.None<string>()
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

        Option.Some("value")
            .Tap(() => someResult = "some", () => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        Option.None<string>()
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

        Option.Some("value")
            .TapSome(value => someResult = value)
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("value");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        Option.None<string>()
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

        Option.Some("value")
            .TapSome(() => someResult = "some")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        someResult = string.Empty;
        noneResult = string.Empty;

        Option.None<string>()
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

        Option.Some("value")
            .TapNone(() => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        Option.None<string>()
            .TapNone(() => noneResult = "none")
            .ShouldBeOfType<Option<string>>();

        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }
}