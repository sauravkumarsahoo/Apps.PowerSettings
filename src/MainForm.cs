using PowerSettings;

namespace Clicksrv.Apps.PowerSettings
{
    public partial class MainForm : Form
    {
        PowerProfileManager PowerProfileManager { get; }
        private readonly ToolStripSeparator separator = new();
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

        public MainForm()
        {
            PowerProfileManager = new();
            InitializeComponent();
            settingsMenuItem.Click += (_, __) => ShowSettings();
            closeMenuItem.Click += (_, __) => Application.Exit();
        }

        private void PowerSettingsForm_Load(object sender, EventArgs e)
        {
            RefreshTray();
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

            trayContextMenu.Items.Add(separator);
            trayContextMenu.Items.Add(settingsMenuItem);
            trayContextMenu.Items.Add(closeMenuItem);
        }

        private void Set(PowerProfile profile)
        {
            PowerProfileManager.SetActiveProfile(profile);
            RefreshTray();
        }

        private void MinimizeToTray()
        {
            WindowState = FormWindowState.Minimized;
            Hide();
            ShowInTaskbar = false;
        }
    }
}
