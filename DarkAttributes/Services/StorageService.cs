using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using DarkAttributes.Infrastructure;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace DarkAttributes.Services
{
    /// <summary>
    ///     A service that provides a reliable storage.
    ///     For example, AppSettings file location can change after VS update. So it's unsafe to store values there.
    /// </summary>
    public class StorageService
    {
        private readonly ShellSettingsManager _shellSettingsManager;

        public StorageService(SVsServiceProvider serviceProvider)
        {
            _shellSettingsManager = new ShellSettingsManager(serviceProvider);
        }

        /// <summary> Preconfigured instance </summary>
        public static StorageService Instance { get; set; }

        #region Int32

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

        #endregion
        
        #region StringArray

        public string[] GetStringArray(string key, string[] defaultValue)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (store.PropertyExists(Constants.ProjectName, key))
            {
                var json = store.GetString(Constants.ProjectName, key);
                return JsonConverter.FromJson<string[]>(json);
            }
            return defaultValue;
        }

        public void SetStringArray(string key, string[] value)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            store.CreateCollection(Constants.ProjectName);

            var serializedValue = JsonConverter.ToJson(value);
            store.SetString(Constants.ProjectName, key, serializedValue);
        }

        #endregion
        
        #region Boolean

        public bool GetBoolean(string key, bool defaultValue)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            return store.GetBoolean(Constants.ProjectName, key, defaultValue);
        }

        public void SetBoolean(string key, bool value)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            store.CreateCollection(Constants.ProjectName);
            store.SetBoolean(Constants.ProjectName, key, value);
        }

        #endregion


        

        
    }
}