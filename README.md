# CSharp.Made.Functional
[![codecov](https://codecov.io/gh/th3oth3rjak3/CSharp.Made.Functional/graph/badge.svg?token=5UF3R55ET6)](https://codecov.io/gh/th3oth3rjak3/CSharp.Made.Functional)

## Add CSharp.Made.Functional

```title="Command Line"
dotnet add package CSharp.Made.Functional
```

## Using statements

The namespace does not match the package name to keep using statements shorter.

```cs title="Usings.cs"
// Base using statement:
global using Functional;

// To use Option types:
global using Functional.Options;

// To use Result types:
global using Functional.Results;

// To use Union types:
global using Functional.Unions;

// For extension methods like Pipe/Tap:
global using Functional.Common;

// For exception handling extensions:
global using static Functional.Exceptions.ExceptionExtensions;

// For static methods like Cons:
global using static Functional.Common.CommonExtensions;

// Other usings for other static methods:
global using static Functional.Options.OptionExtensions;
global using static Functional.Results.ResultExtensions;
```

For examples and more discussion about this library, <a href="https://th3oth3rjak3.github.io/CSharp.Made.Functional" target="_blank" rel="noopener">Read the Docs</a>.
