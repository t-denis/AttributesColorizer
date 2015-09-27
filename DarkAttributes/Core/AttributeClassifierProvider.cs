using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace DarkAttributes.Core
{
    [Export(typeof(IClassifierProvider))]
    [ContentType("CSharp")]
    internal class AttributeClassifierProvider : IClassifierProvider
    {
        private bool _isInitialized;

        [Import]
        private IClassificationTypeRegistryService _classificationRegistry;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            Initialize();
            return buffer.Properties.GetOrCreateSingletonProperty(() => new AttributeClassifier(_classificationRegistry, buffer));
        }

        /// <summary> New instance is created and called every time when a file is opened in VS. </summary>
        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
        }
    }
}
