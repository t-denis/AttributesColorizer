# DarkAttributes
Attributes allow you to add a declarative information to a code.
But in some cases it is convenient to darken that kind of information to be able to focus on the code itself.

![screenshot before](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-before.png)
![screenshot after](https://github.com/t-denis/DarkAttributes/blob/master/Content/screenshot-after.png)

There is an option to filter attributes to darken.
The implementation requires Roslyn to build a syntax tree and a semantic model.
Currently only C# is supported.

### Debug notes
Open project's Properties > Debug

Set "Start external program" to:
`C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe` (or wherever you installed VS)

Set "Command line arguments" to:
`/rootsuffix Exp`
