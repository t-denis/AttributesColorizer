using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace DarkAttributes.Services
{
    /// <summary>
    /// A service that provides a reliable storage.
    /// For example, AppSettings file location can change after VS update. So it's unsafe to store values there.
    /// </summary>
    public class StorageService
    {
        private readonly ShellSettingsManager _shellSettingsManager;
        public StorageService(SVsServiceProvider serviceProvider)
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
