using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DarkAttributes.Core
{
    public class SyntaxTreeProcessor
    {
        [NotNull]
        public IEnumerable<TextSpan> GetAttributeLists([NotNull] SyntaxTree syntaxTree)
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));
            return syntaxTree.GetRoot()
               .DescendantNodes()
               .OfType<AttributeListSyntax>()
               .Select(x => x.Span);
        }
    }
}
