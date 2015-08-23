using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace AttributesColorizer
{
    public class ParsingResult
    {
        public ParsingResult(ITextSnapshot textSnapshot,
            IEnumerable<TextSpan> attributeSpans)
        {
            TextSnapshot = textSnapshot;
            AttributeSpans = attributeSpans.ToImmutableArray();
        }

        public ITextSnapshot TextSnapshot { get; }
        public IReadOnlyCollection<TextSpan> AttributeSpans { get; }
    }
}