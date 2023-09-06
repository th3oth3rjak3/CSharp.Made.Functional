﻿using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
public class Union3Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union3<TypeOne, TypeTwo, TypeThree>(new TypeOne())
            .Match(_ => "type one", _ => "type two", _ => "type three")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union3<TypeOne, TypeTwo, TypeThree>(new TypeTwo())
            .Match(_ => "type one", _ => "type two", _ => "type three")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union3<TypeOne, TypeTwo, TypeThree>(new TypeThree())
            .Match(_ => "type one", _ => "type two", _ => "type three")
            .ShouldBe("type three");
}