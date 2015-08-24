using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DarkAttributes.Core
{
    public class SyntaxTreeProcessor
    {
        public IEnumerable<TextSpan> GetAttributeLists(SyntaxTree syntaxTree)
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));
            return syntaxTree.GetRoot()
               .DescendantNodes()
               .OfType<AttributeListSyntax>()
               .Select(x => x.Span);
        }
    }
}
