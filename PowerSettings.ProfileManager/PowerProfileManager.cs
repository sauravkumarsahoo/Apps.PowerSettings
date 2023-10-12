using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PowerSettings.ProfileManager
{
    public class PowerProfileManager
    {
        private static readonly Regex LineRegex = new(@"(?<Guid>[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12})\s+\((?<Name>.*)\)\s?(?<Active>\*?)?", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly ProcessStartInfo powercfg = new("powercfg", "/list") { CreateNoWindow = true, RedirectStandardOutput = true };

        public List<PowerProfile> PowerProfiles { get; private set; } = new();

        public PowerProfile ActiveProfile => PowerProfiles.First(x => x.Active);

        public PowerProfileManager() => RefreshProfiles();

        public void RefreshProfiles()
        {
            using var powercfg = Process.Start(PowerProfileManager.powercfg);

            string[] powerConfigOutput = powercfg!.StandardOutput.ReadToEnd().Split(Environment.NewLine)[3..^1];
            PowerProfiles = powerConfigOutput
                           .Select(l => LineRegex.Match(l))
                           .Where(m => m.Success)
                           .Select(m => new PowerProfile
                           {
                               Active = m.Groups["Active"].Value.Equals("*"),
                               Name = m.Groups["Name"].Value,
                               Guid = Guid.Parse(m.Groups["Guid"].Value),
                           })
                           .ToList();

            PowerProfiles.ForEach(m => m.Description = PowerManagement.GetSchemeDescription(m.Guid));
        }

        public void SetActiveProfile(PowerProfile profile)
        {
            PowerManagement.SetActiveScheme(profile.Guid);
            RefreshProfiles();
        }
    }
}
