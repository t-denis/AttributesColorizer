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
        public IEnumerable<AttributeListSyntax> GetAttributeListsNodes(SyntaxTree syntaxTree)
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));
            return syntaxTree.GetRoot()
               .DescendantNodes()
               .OfType<AttributeListSyntax>();
        }

        public IEnumerable<TextSpan> GetAttributeLists(SyntaxTree syntaxTree)
        {
            return GetAttributeListsNodes(syntaxTree)
               .Select(x => x.Span);
        }
    }
}
