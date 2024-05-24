namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class FilterTests
{
    // TODO: Try to make these tests better.
    [TestMethod]
    public async Task OptionShouldFilterAsync()
    {
        static bool filterCriteria(string input) => input.Length < 10;
        static Task<bool> filterCriteriaAsync(string input) => filterCriteria(input).Async();

        await Some("short")
            .Async()
            .FilterAsync(filterCriteria)
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await Some("A really long message that should get filtered out")
            .Async()
            .FilterAsync(filterCriteria)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await None<string>()
            .Async()
            .FilterAsync(filterCriteria)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await Some("short")
            .Async()
            .FilterAsync(filterCriteriaAsync)
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await Some("A really long message that should get filtered out")
            .Async()
            .FilterAsync(filterCriteriaAsync)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await None<string>()
            .Async()
            .FilterAsync(filterCriteriaAsync)
            .EffectAsync(option => option.IsNone.ShouldBeTrue());
    }

    [TestMethod]
    public async Task CollectionOfOptionsShouldFilter()
    {
        static bool filterCriteria(int input) => input > 0;
        static Task<bool> filterCriteriaAsync(int input) => Task.FromResult(input > 0);

        IEnumerable<Option<int>> collection = [Some(1), Some(2), None<int>(), Some(0)];

        var filtered = collection.Filter(filterCriteria).ToList();
        filtered.Count().ShouldBe(4);
        filtered[0].ShouldBeEquivalentTo(Some(1));
        filtered[1].ShouldBeEquivalentTo(Some(2));
        filtered[2].ShouldBeEquivalentTo(None<int>());
        filtered[3].ShouldBeEquivalentTo(None<int>());

        filtered =
            await collection
                .Async()
                .FilterAsync(filterCriteria)
                .PipeAsync(values => values.ToList());

        filtered[0].ShouldBeEquivalentTo(Some(1));
        filtered[1].ShouldBeEquivalentTo(Some(2));
        filtered[2].ShouldBeEquivalentTo(None<int>());
        filtered[3].ShouldBeEquivalentTo(None<int>());

        filtered =
            await collection
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
        IEnumerable<Option<int>> collection = [Some(1), Some(2), None<int>(), Some(3)];
        IEnumerable<Option<int>> emptyCollection = [];

        collection.Collect().ShouldBe([1, 2, 3]);
        emptyCollection.Collect().ShouldBe([]);

        var collected =
            await collection
                .Async()
                .CollectAsync();

        collected.ShouldBe([1, 2, 3]);

        collected =
            await emptyCollection
                .Async()
                .CollectAsync();

        collected.ShouldBe([]);
    }
}