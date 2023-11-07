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

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;

        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeOne())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeTwoEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;

        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeTwo())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeThreeEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;

        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeThree())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeTrue();
        typeFourEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeFourEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;

        new Union4<TypeOne, TypeTwo, TypeThree, TypeFour>(new TypeFour())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeTrue();
    }

}
