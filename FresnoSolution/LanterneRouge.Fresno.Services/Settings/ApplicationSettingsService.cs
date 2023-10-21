using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.Utils.Converters;
using log4net;
using Newtonsoft.Json;
using System.Configuration;

namespace LanterneRouge.Fresno.WpfClient.Services.Interfaces
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(ApplicationSettingsService));

        public string? EmailServer
        {
            get => ReadSetting<string>(nameof(EmailServer)); set
            {
                if (value != null)
                {
                    UpsertSetting(nameof(EmailServer), value);
                }
            }
        }

        public int Port { get => ReadSetting<int>(nameof(Port)); set => UpsertSetting(nameof(Port), value.ToString()); }

        public string? Password
        {
            get => ReadSetting<string>(nameof(Password)); set
            {
                if (value != null)
                {
                    UpsertSetting(nameof(Password), value);
                }
            }
        }

        public string? EmailFrom
        {
            get => ReadSetting<string>(nameof(EmailFrom)); set
            {
                if (value != null)
                {
                    UpsertSetting(nameof(EmailFrom), value);
                }
            }
        }

        public string? EmailDisplayName
        {
            get => ReadSetting<string>(nameof(EmailDisplayName));
            set
            {
                if (value != null)
                {
                    UpsertSetting(nameof(EmailDisplayName), value);
                }
            }
        }

        public string? Username
        {
            get => ReadSetting<string>(nameof(Username));
            set
            {
                if (value != null)
                {
                    UpsertSetting(nameof(Username), value);
                }
            }
        }

        public ZoneSettings ZoneSettingsValue { get => ReadSetting<ZoneSettings>(nameof(ZoneSettings)) ?? ZoneSettings.Default; set => UpsertSetting(nameof(ZoneSettings), JsonConvert.SerializeObject(value)); }

        private T? ReadSetting<T>(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings[key] ?? null;
                if (value == null)
                {
                    return default;
                }

                if (typeof(T).Equals(typeof(ZoneSettings)))
                {
                    var zs = JsonConvert.DeserializeObject<ZoneSettings>(value);
                    return (T?)Convert.ChangeType(zs, typeof(T));
                }

                return TConverter.ChangeType<T>(value);
            }

            catch (ConfigurationErrorsException ce)
            {
                Logger.Error(ce.Message, ce);
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

            catch (ConfigurationErrorsException ce)
            {
                Logger.Error("Error writing app settings", ce);
            }
        }
    }
}
