# DarkAttributes
Provides an ability to specify the opacity level for attributes in VS2015.

Attributes allow you to add a declarative information to a code.
But in some cases it is convenient to darken that kind of information to be able to focus on the code itself.

![screenshot before](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-before.png)
![screenshot after](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-after.png)

The implementation requires Roslyn to build a syntax tree and a semantic model.
Currently only C# is supported.
