using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace DarkAttributes
{
    public class DarkAttributesOptionsPage : DialogPage
    {
        [Category("Font settings")]
        [DisplayName("Foreground opacity")]
        [Description("Integer value between 0 (transparent) and 100 (opaque).")]
        public int Opacity { get; set; } = 35;
    }
}
