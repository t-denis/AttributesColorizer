using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace DarkAttributes
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class DarkAttributesOptionsPage : DialogPage
    {
        [Category("Settings")]
        [DisplayName("Foreground opacity")]
        [Description("Integer value between 0 (transparent) and 100 (opaque).")]
        public int ForegroundOpacity { get; set; } = 35;
    }
}
