namespace Functional.Test.Unions;

[TestClass]
[ExcludeFromCodeCoverage]
public class Union8Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeOne())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeTwo())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeThree())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeFour())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type four");

    [TestMethod]
    public void ItShouldMatchCase5() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeFive())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type five");

    [TestMethod]
    public void ItShouldMatchCase6() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeSix())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type six");

    [TestMethod]
    public void ItShouldMatchCase7() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeSeven())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type seven");

    [TestMethod]
    public void ItShouldMatchCase8() =>
        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeEight())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight")
            .ShouldBe("type eight");

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeOne())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeTwoEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeTwo())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeThreeEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeThree())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeTrue();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFourEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeFour())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeTrue();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFiveEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeFive())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeTrue();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeSixEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeSix())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeTrue();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeSevenEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeSeven())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeTrue();
        typeEightEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeEightEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;

        new Union<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight>(new TypeEight())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeTrue();

    }
}
