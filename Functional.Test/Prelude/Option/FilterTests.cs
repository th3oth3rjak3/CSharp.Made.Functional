namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class FilterTests
{
    [TestMethod]
    public async Task OptionShouldFilterAsync()
    {
        await Some("short")
            .Async()
            .FilterAsync(FilterCriteria)
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await Some("A really long message that should get filtered out")
            .Async()
            .FilterAsync(FilterCriteria)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await None<string>()
            .Async()
            .FilterAsync(FilterCriteria)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await Some("short")
            .Async()
            .FilterAsync(FilterCriteriaAsync)
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await Some("A really long message that should get filtered out")
            .Async()
            .FilterAsync(FilterCriteriaAsync)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await None<string>()
            .Async()
            .FilterAsync(FilterCriteriaAsync)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());
        return;

        static Task<bool> FilterCriteriaAsync(string input) => FilterCriteria(input).Async();
        static bool FilterCriteria(string input) => input.Length < 10;
    }

    [TestMethod]
    public async Task CollectionOfOptionsShouldFilter()
    {
        static bool filterCriteria(int input) => input > 0;
        static Task<bool> filterCriteriaAsync(int input) => Task.FromResult(input > 0);

        List<Option<int>> collection = [Some(1), Some(2), None<int>(), Some(0)];

        var filtered = collection.Filter(filterCriteria).ToList();
        filtered.Count().ShouldBe(4);
        filtered[0].ShouldBeEquivalentTo(Some(1));
        filtered[1].ShouldBeEquivalentTo(Some(2));
        filtered[2].ShouldBeEquivalentTo(None<int>());
        filtered[3].ShouldBeEquivalentTo(None<int>());

        filtered =
            await collection
                .AsEnumerable()
                .Async()
                .FilterAsync(filterCriteria)
                .PipeAsync(values => values.ToList());

        filtered[0].ShouldBeEquivalentTo(Some(1));
        filtered[1].ShouldBeEquivalentTo(Some(2));
        filtered[2].ShouldBeEquivalentTo(None<int>());
        filtered[3].ShouldBeEquivalentTo(None<int>());

        filtered =
            await collection
                .AsEnumerable()
                .Async()
                .FilterAsync(filterCriteriaAsync)
                .PipeAsync(values => values.ToList());

        filtered[0].ShouldBeEquivalentTo(Some(1));
        filtered[1].ShouldBeEquivalentTo(Some(2));
        filtered[2].ShouldBeEquivalentTo(None<int>());
        filtered[3].ShouldBeEquivalentTo(None<int>());
    }

    [TestMethod]
    public async Task CollectionOfOptionsShouldCollectSomeValues()
    {
        List<Option<int>> collection = [Some(1), Some(2), None<int>(), Some(3)];

        collection.Collect().ShouldBe([1, 2, 3]);
        Enumerable.Empty<Option<int>>().Collect().ShouldBe([]);

        var collected =
            await collection
                .AsEnumerable()
                .Async()
                .CollectAsync();

        collected.ShouldBe([1, 2, 3]);

        collected =
            await Enumerable
                .Empty<Option<int>>()
                .Async()
                .CollectAsync();

        collected.ShouldBe([]);
    }
}