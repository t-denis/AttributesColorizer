using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AttributesColorizer
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.AttributeClassificationTypeName)]
    [Name("AttributeClassifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AttributeClassifierFormat : ClassificationFormatDefinition
    {
        public AttributeClassifierFormat()
        {
            DisplayName = "Attribute";
            ForegroundOpacity = 0.35;
        }
    }
}
