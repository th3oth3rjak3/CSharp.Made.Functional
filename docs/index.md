# About

## What is this package?

CSharp.Made.Functional is a collection of tools that can assist you in writing code that follows a 
functional style while still using the C# language. There are some abstractions in this project 
that will simplify functional programming in C#.

## Who is it for?

This package is designed for those who prefer to use functional style programming and want to 
be able to incorporate some of the functional modelling principals into code that you work on.

## Why did you create this?

After reading [Domain Modeling Made Functional](https://fsharpforfunandprofit.com/books/#domain-modeling-made-functional) 
by Scott Wlaschin, I became more convinced that [Railway-Oriented Programming](https://fsharpforfunandprofit.com/rop/) 
could help me write better code. As a C# web developer, I kept wishing that C# had some of the same ideas as F# 
when it comes to the type system, especially types like Result and Option. This desire led me to create this 
package in a way that I feel is easy to use and follows many of the naming conventions from F#.

## Does it work with Blazor?

Yes! One of my key needs for a functional library was one with no dependencies that could also be used in 
Blazor WebAssembly projects. This project was developed using .NET 6 and I haven't found any limitations 
to where it can be used yet.
