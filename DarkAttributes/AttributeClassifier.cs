using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DarkAttributes.Core;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes
{
    internal class AttributeClassifier : IClassifier
    {
        private readonly IClassificationType _classificationType;
        private ParsingResult _lastParsingResult;

        internal AttributeClassifier(IClassificationTypeRegistryService registry)
        {
            _classificationType = registry.GetClassificationType(Constants.AttributeClassificationTypeName);
        }

        /// <summary>
        ///     An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        ///     This event gets raised if a non-text change would affect the classification in some way,
        ///     for example typing /* would cause the classification to change in C# without directly
        ///     affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        /// <summary>
        ///     Gets all the <see cref="ClassificationSpan" /> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        ///     This method scans the given SnapshotSpan for potential matches for this classification.
        ///     In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            IReadOnlyCollection<TextSpan> attributeSpans;
            if (_lastParsingResult != null && _lastParsingResult.TextSnapshot == span.Snapshot)
            {
                attributeSpans = _lastParsingResult.AttributeSpans;
            }
            else
            {
                attributeSpans = ParseAttributes(span.Snapshot);
                _lastParsingResult = new ParsingResult(span.Snapshot, attributeSpans);
            }

            var result = new List<ClassificationSpan>();
            var roslynSpan = new TextSpan(span.Start, span.Length);
            foreach (var attribute in attributeSpans)
            {
                if (attribute.OverlapsWith(roslynSpan))
                {
                    var snapshotSpan = new SnapshotSpan(span.Snapshot, attribute.Start, attribute.Length);
                    var classificationSpan = new ClassificationSpan(snapshotSpan, _classificationType);
                    result.Add(classificationSpan);
                }
            }

            return result;
        }

        private static IReadOnlyCollection<TextSpan> ParseAttributes(ITextSnapshot snapshot)
        {
            var document = snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var syntaxTree = document.GetSyntaxTreeAsync().Result;

            var codeParser = new SyntaxTreeProcessor();
            var attributes = codeParser.GetAttributeLists(syntaxTree);
            return attributes.ToImmutableList();
        }
    }
}