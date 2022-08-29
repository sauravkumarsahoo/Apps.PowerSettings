using PowerSettings;
using System.Diagnostics;

namespace Clicksrv.Apps.PowerSettings
{
    public partial class MainForm : Form
    {
        PowerProfileManager PowerProfileManager { get; }

        private readonly static string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        private readonly static ProcessStartInfo poweroptions = new()
        {
            FileName = $@"{windir}\system32\control.exe",
            Arguments = "/name Microsoft.PowerOptions"
        };

        private readonly ToolStripSeparator separator = new();
        private readonly ToolStripSeparator separator2 = new();
        private readonly ToolStripMenuItem settingsMenuItem = new()
        {
            Text = "Settings",
            CheckOnClick = false,
        };
        private readonly ToolStripMenuItem closeMenuItem = new()
        {
            Text = "Exit",
            CheckOnClick = false,
        };
        private readonly ToolStripMenuItem openPowerOptionsMenuItem = new()
        {
            Text = "Show Power Options",
            CheckOnClick = false,
        };

        public MainForm()
        {
            PowerProfileManager = new();
            InitializeComponent();
            settingsMenuItem.Click += (_, __) => ShowSettings();
            closeMenuItem.Click += (_, __) => Application.Exit();
            openPowerOptionsMenuItem.Click += (_, __) => Process.Start(poweroptions);
            RefreshTray();
        }

        private void PowerSettingsForm_Load(object sender, EventArgs e)
        {
            MinimizeToTray();
        }


        private void ShowSettings()
        {
            SettingsForm settingsForm = new(notifyIcon);
            settingsForm.ShowDialog();
        }

        private void RefreshTray()
        {
            notifyIcon.Icon = ThemeHelper.ShouldSystemUseDarkMode() ? Icons.tray_dark : Icons.tray_light;

            trayContextMenu.Items.Clear();
            trayContextMenu.Items.Add(openPowerOptionsMenuItem);
            trayContextMenu.Items.Add(separator);

            foreach (var profile in PowerProfileManager.PowerProfiles)
            {
                var tsItem = new ToolStripMenuItem
                {
                    Text = profile.Name,
                    Checked = profile.Active
                };

                tsItem.Click += (_, __) => Set(PowerProfileManager.PowerProfiles.First(x => x.Name == profile.Name));
                trayContextMenu.Items.Add(tsItem);

                if (profile.Active)
                    notifyIcon.Text = $"{profile.Name}";
            }

            trayContextMenu.Items.Add(separator2);
            trayContextMenu.Items.Add(settingsMenuItem);
            trayContextMenu.Items.Add(closeMenuItem);
        }

        private void Set(PowerProfile profile)
        {
            PowerProfileManager.SetActiveProfile(profile);
            notifyIcon.Text = $"{profile.Name}";
        }

        private void MinimizeToTray()
        {
            WindowState = FormWindowState.Minimized;
            Hide();
            ShowInTaskbar = false;
        }

        private void OnTrayOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefreshTray();
        }
    }
}
