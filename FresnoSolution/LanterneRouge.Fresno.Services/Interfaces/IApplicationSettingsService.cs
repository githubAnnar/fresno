using LanterneRouge.Fresno.WpfClient.Services.Settings;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public interface IApplicationSettingsService
    {
        string? EmailServer { get; set; }

        int Port { get; set; }

        string? Password { get; set; }

        string? EmailFrom { get; set; }

        string? EmailDisplayName { get; set; }

        string? Username { get; set; }

        ZoneSettings ZoneSettingsValue { get; set; }
    }
}
