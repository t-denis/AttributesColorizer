﻿using System;
using System.Collections.Generic;
using AttributesColorizer.Core;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace AttributesColorizer
{
    internal class AttributeClassifier : IClassifier
    {
        private readonly IClassificationType _classificationType;

        internal AttributeClassifier(IClassificationTypeRegistryService registry)
        {
            _classificationType = registry.GetClassificationType(Constants.AttributeClassificationTypeName);
        }

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>();
            var document = span.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            var syntaxTree = document.GetSyntaxTreeAsync().Result;

            var codeParser = new CodeParser();
            var attributes = codeParser.GetAttributeLists(syntaxTree);
            var roslynSpan = new TextSpan(span.Start, span.Length);
            foreach (var attribute in attributes)
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
    }
}
