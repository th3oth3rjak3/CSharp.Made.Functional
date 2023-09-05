using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
public class UnionTests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union<TypeOne, TypeTwo>(new TypeOne())
            .Match(_ => "type one", _ => "type two")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union<TypeOne, TypeTwo>(new TypeTwo())
            .Match(_ => "type one", _ => "type two")
            .ShouldBe("type two");
}

