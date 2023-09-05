using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
public class Union5Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
    new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeOne())
        .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
        .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeTwo())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeThree())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFour())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type four");

    [TestMethod]
    public void ItShouldMatchCase5() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFive())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type five");

}
