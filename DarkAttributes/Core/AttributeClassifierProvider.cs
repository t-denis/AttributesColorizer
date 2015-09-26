using System.ComponentModel.Composition;
using DarkAttributes.Services;
using Microsoft.VisualStudio.Shell;
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
        [Import]
        private SVsServiceProvider _serviceProvider;
        [Import]
        private IClassificationFormatMapService _classicationFormatMapService;
        [Import]
        private IClassificationTypeRegistryService _classificationTypeRegistryService;


        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            Initialize();
            return buffer.Properties.GetOrCreateSingletonProperty(() => new AttributeClassifier(_classificationRegistry));
        }

        /// <summary>
        /// It is called every time when user opens a file in VS.
        /// </summary>
        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            var textPropertiesService = new TextPropertiesService(_classificationTypeRegistryService, _classicationFormatMapService);
            var store = new StorageService(_serviceProvider);
            var opacity = store.GetInt32(Constants.StorageKeys.ForegroundOpacity, 35);
            textPropertiesService.SetForegroundOpacity(opacity / 100.0);
            _isInitialized = true;
        }
    }
}
