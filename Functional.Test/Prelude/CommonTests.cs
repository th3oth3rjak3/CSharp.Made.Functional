namespace Functional.Test.Prelude;

[TestClass]
public class CommonTests
{
    [TestMethod]
    public async Task ItShouldCreateTasksWithAsync()
    {
        var task = "something".Async();

        await task.AssertInstanceOfType(typeof(Task<string>));

        (await task).ShouldBe("something");
    }

    [TestMethod]
    public async Task ItShouldIgnoreAsync()
    {
        var ignore = "hello".Async();

        await ignore.IgnoreAsync().AssertInstanceOfType(typeof(Task));
    }

    [TestMethod]
    public void PreludeUnitShouldReturnUnit()
    {

        Unit unit = Unit();
        unit.AssertInstanceOfType(typeof(Unit));
        unit.ShouldBe(Unit.Default);
    }
}