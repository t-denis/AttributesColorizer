using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace DarkAttributes
{
    public class ExtensionSettings
    {
        private readonly Properties _properties;

        public ExtensionSettings()
        {
            var dte = (DTE)Package.GetGlobalService(typeof(DTE));
            _properties = dte.Properties["DarkAttributes", "General"];
        }

        public bool ForegroundOpacity => (bool)_properties.Item("ForegroundOpacity").Value;
    }
}