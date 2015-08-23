# AttributesColorizer
Provides an ability to specify a font for attributes in VS2015.

Attributes used to provide declarative information about the code.
In some cases it is convenient to darken that kind of information to be able to focus on the code itself.

![screenshot before](https://github.com/t-denis/AttributesColorizer/blob/master/Content/screenshot-before.png)
![screenshot after](https://github.com/t-denis/AttributesColorizer/blob/master/Content/screenshot-after.png)

The project requires Roslyn to build the syntax tree. Probably it will also use Roslyn to get a semantic model for further analysis, for example to implement a black/white list of attributes to be processed.
