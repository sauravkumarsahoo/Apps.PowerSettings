using System.Runtime.InteropServices;

namespace PowerSettings
{
    internal class ThemeHelper
    {

        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        public static extern bool ShouldSystemUseDarkMode();
    }
}
