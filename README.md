# DarkAttributes
Provides an ability to specify a font for attributes in VS2015.

Attributes used to provide declarative information about the code.
In some cases it is convenient to darken that kind of information to be able to focus on the code itself.

![screenshot before](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-before.png)
![screenshot after](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-after.png)

The implementation requires Roslyn to build a syntax tree and a semantic model.
Currently only C# is supported.
