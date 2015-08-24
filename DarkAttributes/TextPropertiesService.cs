using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes
{
    public class TextPropertiesService
    {
        private readonly IClassificationTypeRegistryService _registryService;
        private readonly IClassificationFormatMapService _classificationFormatMapService;

        public TextPropertiesService(IClassificationTypeRegistryService registryService,
            IClassificationFormatMapService classificationFormatMapService)
        {
            _registryService = registryService;
            _classificationFormatMapService = classificationFormatMapService;
        }

        public void SetForegroundOpacity(double opacity)
        {
            var classificationType = _registryService.GetClassificationType(Constants.AttributeClassificationTypeName);
            var classificationFormatMap = _classificationFormatMapService.GetClassificationFormatMap("CSharp");
            var currentProperties = classificationFormatMap.GetExplicitTextProperties(classificationType);
            if (currentProperties.ForegroundOpacity == opacity)
                return;
            var newProperties = currentProperties.SetForegroundOpacity(opacity);
            classificationFormatMap.AddExplicitTextProperties(classificationType, newProperties);
        }
    }
}
