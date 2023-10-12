using Clicksrv.Packages.StartWithOSSettings.Windows;

namespace PowerSettings
{
    public partial class SettingsForm : Form
    {
        private readonly WindowsStartupOptions startupOptions = new("PowerSettings", Application.ExecutablePath, Array.Empty<string>(), global: false);
        private readonly NotifyIcon trayIconControl;

        public SettingsForm(NotifyIcon trayIconControl)
        {
            InitializeComponent();
            this.trayIconControl = trayIconControl;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            startWithWindowsToggle.Checked = startupOptions.Enabled;
        }

        private void startWithWindowsToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (startWithWindowsToggle.Checked)
                startupOptions.Enable();
            else
                startupOptions.Disable();
        }
    }
}
