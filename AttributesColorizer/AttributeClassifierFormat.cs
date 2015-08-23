using System.ComponentModel.Composition;
using System.Windows.Media;
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
            BackgroundColor = Colors.BlueViolet;
            TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
}
