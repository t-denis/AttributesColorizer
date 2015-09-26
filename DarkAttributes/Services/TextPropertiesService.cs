using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes.Services
{
    public class TextPropertiesService
    {
        private readonly IClassificationFormatMap _classificationFormatMap;
        private readonly IClassificationType _classificationType;

        public TextPropertiesService(IClassificationTypeRegistryService registryService,
            IClassificationFormatMapService classificationFormatMapService)
        {
            _classificationFormatMap = classificationFormatMapService.GetClassificationFormatMap("text");
            _classificationType = registryService.GetClassificationType(Constants.AttributeClassificationTypeName);
        }

        public double GetForegroundOpacity()
        {
            var currentProperties = _classificationFormatMap.GetExplicitTextProperties(_classificationType);
            return currentProperties.ForegroundOpacity;
        }

        public void SetForegroundOpacity(double opacity)
        {
            var currentProperties = _classificationFormatMap.GetExplicitTextProperties(_classificationType);
            if (currentProperties.ForegroundOpacity == opacity)
                return;
            var newProperties = currentProperties.SetForegroundOpacity(opacity);

            _classificationFormatMap.AddExplicitTextProperties(_classificationType, newProperties);
        }
    }
}
