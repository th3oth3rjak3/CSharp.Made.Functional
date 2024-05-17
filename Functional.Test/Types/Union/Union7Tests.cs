//using System.Diagnostics.CodeAnalysis;

//using Functional.Unions;

//namespace Functional.Test.Unions;

//[TestClass]
//[ExcludeFromCodeCoverage]
//public class Union7Tests
//{
//    [TestMethod]
//    public void ItShouldMatchCase1() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeOne())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type one");

//    [TestMethod]
//    public void ItShouldMatchCase2() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeTwo())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type two");

//    [TestMethod]
//    public void ItShouldMatchCase3() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeThree())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type three");

//    [TestMethod]
//    public void ItShouldMatchCase4() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeFour())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type four");

//    [TestMethod]
//    public void ItShouldMatchCase5() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeFive())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type five");

//    [TestMethod]
//    public void ItShouldMatchCase6() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeSix())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type six");

//    [TestMethod]
//    public void ItShouldMatchCase7() =>
//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeSeven())
//            .Match(
//                _ => "type one",
//                _ => "type two",
//                _ => "type three",
//                _ => "type four",
//                _ => "type five",
//                _ => "type six",
//                _ => "type seven")
//            .ShouldBe("type seven");

//    [TestMethod]
//    public void ItShouldPerformTypeOneEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeOne())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeTrue();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeFalse();
//    }

//    [TestMethod]
//    public void ItShouldPerformTypeTwoEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeTwo())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeTrue();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeFalse();

//    }

//    [TestMethod]
//    public void ItShouldPerformTypeThreeEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeThree())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeTrue();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeFalse();

//    }

//    [TestMethod]
//    public void ItShouldPerformTypeFourEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeFour())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeTrue();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeFalse();

//    }

//    [TestMethod]
//    public void ItShouldPerformTypeFiveEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeFive())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeTrue();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeFalse();

//    }

//    [TestMethod]
//    public void ItShouldPerformTypeSixEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeSix())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeTrue();
//        typeSevenEffect.ShouldBeFalse();

//    }

//    [TestMethod]
//    public void ItShouldPerformTypeSevenEffect()
//    {
//        var typeOneEffect = false;
//        var typeTwoEffect = false;
//        var typeThreeEffect = false;
//        var typeFourEffect = false;
//        var typeFiveEffect = false;
//        var typeSixEffect = false;
//        var typeSevenEffect = false;

//        new Union7<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven>(new TypeSeven())
//            .Effect(
//                _ => typeOneEffect = true,
//                _ => typeTwoEffect = true,
//                _ => typeThreeEffect = true,
//                _ => typeFourEffect = true,
//                _ => typeFiveEffect = true,
//                _ => typeSixEffect = true,
//                _ => typeSevenEffect = true);

//        typeOneEffect.ShouldBeFalse();
//        typeTwoEffect.ShouldBeFalse();
//        typeThreeEffect.ShouldBeFalse();
//        typeFourEffect.ShouldBeFalse();
//        typeFiveEffect.ShouldBeFalse();
//        typeSixEffect.ShouldBeFalse();
//        typeSevenEffect.ShouldBeTrue();

//    }
//}
