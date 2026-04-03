using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace PowerSettings.CLI
{

    public class DisplayMonitorInfo
    {
        public string Name { get; set; }
        public List<string> Resolutions { get; set; }
        public string CurrentResolution { get; set; }
        public string DisplayMode { get; set; }
        public string MonitorArrangement { get; set; }
    }

    public class DisplayMonitorManager
    {
        public List<DisplayMonitorInfo> GetConnectedMonitorsInfo()
        {
            List<DisplayMonitorInfo> monitorsInfo = new List<DisplayMonitorInfo>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
            foreach (ManagementObject monitor in searcher.Get())
            {
                DisplayMonitorInfo monitorInfo = new DisplayMonitorInfo();
                monitorInfo.Name = monitor["Name"].ToString();

                List<string> resolutions = new List<string>();

                var props = new Dictionary<string, string>();

                foreach (PropertyData property in monitor.Properties)
                {
                    props.Add(property.Name, property.Value?.ToString() ?? string.Empty);

                    if (property.Name == "DisplayModesSupported")
                    {
                        Array displayModes = (Array)property.Value;
                        foreach (var mode in displayModes)
                        {
                            resolutions.Add(mode.ToString());
                        }
                    }
                    else if (property.Name == "CurrentHorizontalResolution")
                    {
                        monitorInfo.CurrentResolution = property.Value.ToString();
                    }
                }
                monitorInfo.Resolutions = resolutions;

                monitorsInfo.Add(monitorInfo);
            }

            return monitorsInfo;
        }

        public void SetDisplayMode(int displayMode)
        {
            // Use appropriate code to set the display mode based on the provided displayMode value
            // For example, you can use the Windows API functions like ChangeDisplaySettings or SetDisplayConfig
        }
    }
}
