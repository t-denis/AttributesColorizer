using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AttributesColorizer.Core
{
    public class CodeParser
    {
        [NotNull]
        public IEnumerable<TextSpan> GetAttributes([NotNull] SyntaxTree syntaxTree)
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));
            return syntaxTree.GetRoot()
               .DescendantNodes()
               .OfType<AttributeListSyntax>()
               .Select(x => x.Span);
        }
    }
}
