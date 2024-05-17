namespace Functional.Test.Prelude.Common;

[TestClass]
public class CommonTests
{
    [TestMethod]
    public async Task ItShouldCreateTasksWithAsync()
    {
        var task = "something".Async();

        task.AssertInstanceOfType(typeof(Task<string>));

        (await task).ShouldBe("something");
    }

    [TestMethod]
    public void ItShouldIgnoreAsync()
    {
        var ignore = "hello".Async();

        ignore.IgnoreAsync().AssertInstanceOfType(typeof(Task));
    }

}
