using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace DarkAttributes.Settings
{
    public class SettingsStore
    {
        private readonly ShellSettingsManager _shellSettingsManager;
        public SettingsStore(SVsServiceProvider serviceProvider)
        {
            _shellSettingsManager = new ShellSettingsManager(serviceProvider);
        }

        public int GetInt32(string key, int defaultValue)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            return store.GetInt32(Constants.ProjectName, key, defaultValue);
        }

        public void SetInt32(string key, int value)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            store.CreateCollection(Constants.ProjectName);
            store.SetInt32(Constants.ProjectName, key, value);
        }
    }
}
