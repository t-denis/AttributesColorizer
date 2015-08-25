using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;

namespace DarkAttributes
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class DarkAttributesOptionsPage : DialogPage
    {
        private readonly TextPropertiesService _textPropertiesService;
        private int _foregroundOpacity;

        public DarkAttributesOptionsPage()
        {
            var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            var classicationFormatMapService = componentModel.GetService<IClassificationFormatMapService>();
            var classificationTypeRegistryService = componentModel.GetService<IClassificationTypeRegistryService>();
            _textPropertiesService = new TextPropertiesService(classificationTypeRegistryService, classicationFormatMapService);
        }

        [Category("Settings")]
        [DisplayName("Foreground opacity")]
        [Description("Integer value between 0 (transparent) and 100 (opaque).")]
        public int ForegroundOpacity
        {
            get { return _foregroundOpacity; }
            set { _foregroundOpacity = value; }
        }

        public override void SaveSettingsToStorage()
        {
            _textPropertiesService.SetForegroundOpacity(_foregroundOpacity / 100.0);
        }

        public override void LoadSettingsFromStorage()
        {
            _foregroundOpacity = Convert.ToInt32(_textPropertiesService.GetForegroundOpacity() * 100.0);
        }
    }
}