using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.WpfClient.Utils;
using Newtonsoft.Json;
using System;
using System.Configuration;

namespace LanterneRouge.Fresno.WpfClient.Services.Interfaces
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public string EmailServer { get => ReadSetting<string>(nameof(EmailServer)); set => UpsertSetting(nameof(EmailServer), value); }

        public int Port { get => ReadSetting<int>(nameof(Port)); set => UpsertSetting(nameof(Port), value.ToString()); }

        public string Password { get => ReadSetting<string>(nameof(Password)); set => UpsertSetting(nameof(Password), value); }

        public string EmailFrom { get => ReadSetting<string>(nameof(EmailFrom)); set => UpsertSetting(nameof(EmailFrom), value); }

        public string EmailDisplayName { get => ReadSetting<string>(nameof(EmailDisplayName)); set => UpsertSetting(nameof(EmailDisplayName), value); }

        public string Username { get => ReadSetting<string>(nameof(Username)); set => UpsertSetting(nameof(Username), value); }

        public ZoneSettings ZoneSettingsValue { get => ReadSetting<ZoneSettings>(nameof(ZoneSettings)) ?? ZoneSettings.Default; set => UpsertSetting(nameof(ZoneSettings), JsonConvert.SerializeObject(value)); }

        private T ReadSetting<T>(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings[key] ?? null;
                if (value == null)
                {
                    return default;
                }

                return TConverter.ChangeType<T>(value);
            }

            catch (ConfigurationErrorsException ce)
            {
                Console.WriteLine(ce.Message);
                return default;
            }
        }

        private void UpsertSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }

                else
                {
                    settings[key].Value = value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }

            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
