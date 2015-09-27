using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
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

        public string[] GetStringArray(string key, string[] defaultValue)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (store.PropertyExists(Constants.ProjectName, key))
            {
                var json = store.GetString(Constants.ProjectName, key);
                return FromJson<string[]>(json);
            }
            return defaultValue;
        }

        public void SetStringArray(string key, string[] value)
        {
            var store = _shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            store.CreateCollection(Constants.ProjectName);

            var serializedValue = ToJson(value);
            store.SetString(Constants.ProjectName, key, serializedValue);
        }

        private static string ToJson<T>(T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        private static T FromJson<T>(string json) where T : class
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}