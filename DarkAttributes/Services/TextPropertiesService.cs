﻿using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes.Services
{
    /// <summary>
    /// A service that works with properties of the classification type.
    /// VS drawing engine uses these values to colorize text.
    /// </summary>
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
