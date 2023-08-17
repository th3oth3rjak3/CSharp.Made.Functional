
using System.Diagnostics.CodeAnalysis;

using Functional.Monadic;
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
            .ShouldBe(new None<string>());

    [TestMethod]
    public void SomeShouldBeOfOptionType() =>
        1.Optional()
            .ShouldBe(new Some<int>(1));

    [TestMethod]
    public void BindMethodShouldTransformOptionTypesWhenSome() =>
        1.Optional()
            .Map((value) => value.ToString())
            .ShouldBe(new Some<string>("1"));

    [TestMethod]
    public void BindShouldTransformOptionTypesWhenNone() =>
        Option.None<int>()
            .Map((value) => value.ToString())
            .ShouldBe(new None<string>());

    [TestMethod]
    public void FilterShouldFindSomeWhenTrue() =>
        "findme".Some()
            .Filter(value => value == "findme")
            .ShouldBe(new Some<string>("findme"));

    [TestMethod]
    public void FilterSHouldNotFindSomeWhenFalse() =>
        "cantfindme".Some()
            .Filter(value => value == "findme")
            .ShouldBe(new None<string>());

    [TestMethod]
    public void FilterShouldNotFindNone() =>
        Option.None<string>()
            .Filter(value => value == "findme")
            .ShouldBe(new None<string>());

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
    public void ReduceShouldUseDelagateValueWhenNone() =>
        Option.None<string>()
            .Reduce(() => "other delegate")
            .ShouldBe("other delegate");

    [TestMethod]
    public void NoneOfMethodShouldCreateNone() =>
        Option.None<string>()
            .ShouldBe(new None<string>());

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
    public async Task ItShouldReduceSomeAsyncWithFuncs() =>
        await 1
            .Optional()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldReduceNoneAsyncWithFuncs() =>
        await Option.None<int>()
            .AsAsync()
            .MapAsync(val => val.ToString())
            .ReduceAsync(() => "something else")
            .TapAsync(str => str.ShouldBe("something else"));

}
