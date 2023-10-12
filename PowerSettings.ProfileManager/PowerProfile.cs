namespace PowerSettings.ProfileManager
{
    public class PowerProfile
    {
        public bool Active { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid Guid { get; set; }
        public string Description { get; set; } = string.Empty;

        public override bool Equals(object? obj) => (obj?.GetHashCode() ?? 0) == GetHashCode();
        public override int GetHashCode() => Guid.GetHashCode();
    }
}