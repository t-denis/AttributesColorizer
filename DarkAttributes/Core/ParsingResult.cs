using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace DarkAttributes.Core
{
    public class ParsingResult
    {
        public ParsingResult(Settings settings,
            ITextSnapshot textSnapshot,
            IEnumerable<TextSpan> attributeSpans)
        {
            TextSnapshot = textSnapshot;
            AttributeSpans = attributeSpans.ToImmutableArray();
        }

        public Settings Settings { get; }
        public ITextSnapshot TextSnapshot { get; }
        public IReadOnlyCollection<TextSpan> AttributeSpans { get; }
    }
}