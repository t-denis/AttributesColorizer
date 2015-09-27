using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DarkAttributes.Services;
using Microsoft.VisualStudio.Shell;

namespace DarkAttributes.Pages
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class DarkAttributesOptionsPage : DialogPage
    {
        [Category("1. Visual settings")]
        [DisplayName("1. Foreground opacity")]
        [Description("Integer value between 0 (transparent) and 100 (opaque).")]
        public int ForegroundOpacity { get; set; }

        [Category("2. Filter")]
        [DisplayName("1. Enable filter")]
        [Description("Filter attributes to darken. Otherwise all attributes will be darkened.")]
        public bool EnableFilters { get; set; }


        [Category("2. Filter")]
        [DisplayName("2. List of attributes to darken")]
        [Description("A list of attributes to darken. Attributes can be specified in the following ways: " +
                     "short name (Display), name (DisplayAttribute), " +
                     "full name (System.ComponentModel.DataAnnotations.DisplayAttribute) " +
                     "or by using wildcards (System.ComponentModel.DataAnnotations.*)")]
        public string[] Blacklist { get; set; }
        
        public override void SaveSettingsToStorage()
        {
            if (ForegroundOpacity < 0)
                ForegroundOpacity = 0;
            if (ForegroundOpacity > 100)
                ForegroundOpacity = 100;
            StorageService.Instance.SetInt32(Constants.StorageKeys.ForegroundOpacity, ForegroundOpacity);
            StorageService.Instance.SetBoolean(Constants.StorageKeys.EnableFilters, EnableFilters);
            StorageService.Instance.SetStringArray(Constants.StorageKeys.Blacklist, Blacklist);
            TextPropertiesService.Instance.UpdateTextPropertiesFromStorage();
        }

        public override void LoadSettingsFromStorage()
        {
            ForegroundOpacity = StorageService.Instance.GetInt32(Constants.StorageKeys.ForegroundOpacity,
                Constants.DefaultForegroundOpacity);
            EnableFilters = StorageService.Instance.GetBoolean(Constants.StorageKeys.EnableFilters, false);
            Blacklist = StorageService.Instance.GetStringArray(Constants.StorageKeys.Blacklist, null);
        }
    }
}