using System;
using DarkAttributes.Services;

namespace DarkAttributes
{
    public class Settings
    {
        public int Opacity { get; set; }
        public bool IsFilteringEnabled { get; set; }
        public string[] Blacklist { get; set; }

        public static Settings Load()
        {
            var storage = StorageService.Instance;
            if (storage == null)
                throw new InvalidOperationException("StorageService is not initialized");
            var settings = new Settings
            {
                Opacity = storage.GetInt32(StorageKeys.ForegroundOpacity, Defaults.Opacity),
                IsFilteringEnabled = storage.GetBoolean(StorageKeys.IsFilteringEnabled, Defaults.IsFilteringEnabled),
                Blacklist = storage.GetStringArray(StorageKeys.Blacklist, Defaults.Blacklist)
            };
            return settings;
        }

        public static void Save(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var storage = StorageService.Instance;
            if (storage == null)
                throw new InvalidOperationException("StorageService is not initialized");
            storage.SetInt32(StorageKeys.ForegroundOpacity, settings.Opacity);
            storage.SetBoolean(StorageKeys.IsFilteringEnabled, settings.IsFilteringEnabled);
            storage.SetStringArray(StorageKeys.Blacklist, settings.Blacklist);

            var textPropertiesService = TextPropertiesService.Instance;
            if (textPropertiesService == null)
                throw new InvalidOperationException("TextPropertiesService not initialized");
            textPropertiesService.UpdateTextPropertiesFromSettings();
        }

        private static class Defaults
        {
            public const int Opacity = 40;
            public const bool IsFilteringEnabled = false;
            public const string[] Blacklist = null;
        }

        private static class StorageKeys
        {
            public const string ForegroundOpacity = "ForegroundOpacity";
            public const string IsFilteringEnabled = "IsFilteringEnabled";
            public const string Blacklist = "AttributesBlacklist";
        }
    }
}
