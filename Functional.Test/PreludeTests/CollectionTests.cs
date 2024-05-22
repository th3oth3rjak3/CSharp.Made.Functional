namespace Functional.Test.PreludeTests;

[TestClass]
public class CollectionTests
{
    [TestMethod]
    public void ConsShouldAddItemsToAList() =>
        Cons("1", "2")
        .Tap(
            list => list.AssertInstanceOfType(typeof(IEnumerable<string>)),
            list => list.Count().ShouldBe(2));

    [TestMethod]
    public void ItShouldAppendItems()
    {
        var collection = Cons("1", "2");
        var items = Cons("3", "4");

        collection.Append(items).Count().ShouldBe(4);
        collection.Count().ShouldBe(2);

        collection.Append("3", "4").Count().ShouldBe(4);
        collection.Count().ShouldBe(2);
    }
}
