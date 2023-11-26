
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
        1.Optional()
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
            .TapAsync(result => result.ShouldBeEquivalentTo(Option.Some("input")));

    [TestMethod]
    public async Task ItShouldMapOptionOfTaskOfString() =>
        await Option.Some("input")
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
}