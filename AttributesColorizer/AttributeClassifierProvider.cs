using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AttributesColorizer
{
    [Export(typeof(IClassifierProvider))]
    [ContentType("CSharp")]
    internal class AttributeClassifierProvider : IClassifierProvider
    {
        [Import]
        private IClassificationTypeRegistryService _classificationRegistry;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new AttributeClassifier(_classificationRegistry));
        }
    }
}
