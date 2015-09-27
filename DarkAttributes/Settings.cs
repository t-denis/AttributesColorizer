using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DarkAttributes.Messages;
using DarkAttributes.Services;

namespace DarkAttributes
{
    public class Settings
    {
        public int Opacity { get; }
        public bool IsFilteringEnabled { get; }
        public IReadOnlyList<string> Blacklist { get; }

        public Settings(int opacity, bool isFilteringEnabled, IEnumerable<string> blacklist)
        {
            Opacity = opacity;
            IsFilteringEnabled = isFilteringEnabled;
            Blacklist = blacklist.ToImmutableList();
        }

        public static Settings Load()
        {
            var storage = StorageService.Instance;
            if (storage == null)
                throw new InvalidOperationException("StorageService is not initialized");

            var opacity = storage.GetInt32(StorageKeys.ForegroundOpacity, Defaults.Opacity);
            var isFilteringEnabled = storage.GetBoolean(StorageKeys.IsFilteringEnabled, Defaults.IsFilteringEnabled);
            var blacklist = storage.GetStringArray(StorageKeys.Blacklist, Defaults.Blacklist);

            return new Settings(opacity, isFilteringEnabled, blacklist);
        }

        public static void Save(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var storage = StorageService.Instance;
            if (storage == null)
                throw new InvalidOperationException("StorageService is not initialized");
            storage.SetInt32(StorageKeys.ForegroundOpacity, settings.Opacity);
            storage.SetBoolean(StorageKeys.IsFilteringEnabled, settings.IsFilteringEnabled);
            storage.SetStringArray(StorageKeys.Blacklist, settings.Blacklist?.ToArray());

            var textPropertiesService = TextPropertiesService.Instance;
            if (textPropertiesService == null)
                throw new InvalidOperationException("TextPropertiesService not initialized");
            textPropertiesService.UpdateTextPropertiesFromSettings();
            Bus.Publish(new SettingsChangedMessage());
        }

        private static class Defaults
        {
            public const int Opacity = 40;
            public const bool IsFilteringEnabled = false;
            public static readonly string[] Blacklist =
            {
                "System.ComponentModel.*",
                "System.Runtime.Serialization.*",
                "JetBrains.Annotations.*"
            };
        }

        private static class StorageKeys
        {
            public const string ForegroundOpacity = "ForegroundOpacity";
            public const string IsFilteringEnabled = "IsFilteringEnabled";
            public const string Blacklist = "AttributesBlacklist";
        }

        #region Equals

        protected bool Equals(Settings other)
        {
            return Opacity == other.Opacity && IsFilteringEnabled == other.IsFilteringEnabled &&
                   Equals(Blacklist, other.Blacklist);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Settings) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Opacity;
                hashCode = (hashCode*397) ^ IsFilteringEnabled.GetHashCode();
                hashCode = (hashCode*397) ^ (Blacklist != null ? Blacklist.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}
