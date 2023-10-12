using PowerSettings.ProfileManager;
using System.Xml.Linq;

if (args.Length > 1)
{
    Console.Error.WriteLine("Unknown parameters were passed.");
    Console.WriteLine("\nUsage:\nDo not pass arguments to go into interactive mode.\nPass a single argument with the correct profile name (enclosed in quotes if name has a space) to be set.");
    return 1;
}


if (args.Length == 1)
{
    try
    {
        PowerManagement.SetActiveScheme(args[0]);
    } catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return 2;
    }

    Console.WriteLine("Powerscheme [" + args[0] + "] is now active.");
    return 0;
}

var manager = new PowerProfileManager();

var profiles = manager.PowerProfiles;
var profilePrompts = profiles.Select((profile, index) => $"{index + 1}. {profile.Name}");
var prompt = $"Welcome to PowerSettings!\n\n{string.Join(Environment.NewLine, profilePrompts)}\n\nEnter the sequence number of the power scheme to be selected: " ;

Console.Write(prompt);

var input = Console.ReadLine()?.Trim();
int result = -1;

while (string.IsNullOrEmpty(input) 
    || !int.TryParse(input, out result) 
    || result <= 1
    || result > profiles.Count)
{
    Console.Write("\nInput entered was invalid, please try again.\nEnter the sequence number of the power scheme to be selected: ");
    input = Console.ReadLine()?.Trim();
}

var profileToBeSelected = profiles[result - 1];
manager.SetActiveProfile(profileToBeSelected);

Console.WriteLine("Powerscheme [" + profileToBeSelected.Name + "] is now active.");

return 0;