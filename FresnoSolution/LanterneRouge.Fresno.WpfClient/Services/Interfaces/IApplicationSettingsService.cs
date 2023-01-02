using LanterneRouge.Fresno.Calculations;

namespace LanterneRouge.Fresno.WpfClient.Services.Interfaces
{
    public interface IApplicationSettingsService
    {
        string EmailServer { get; set; }

        int Port { get; set; }

        string Password { get; set; }

        string EmailFrom { get; set; }

        string EmailDisplayName { get; set; }

        string Username { get; set; }

        ZoneSettings ZoneSettingsValue { get; set; }
    }
}
