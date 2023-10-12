using System.Runtime.InteropServices;

namespace PowerSettings.App
{
    internal class ThemeHelper
    {

        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        public static extern bool ShouldSystemUseDarkMode();
    }
}
