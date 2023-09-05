using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
public class Union4Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeOne())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeTwo())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeThree())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeFour())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four")
            .ShouldBe("type four");
}
