using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services.Settings;
using System.Collections.ObjectModel;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.UserControls.ViewModel
{
    public class ZoneSettingsViewModel : ViewModelBase
    {
        private string _name;

        public ZoneSettingsViewModel(ZoneSetting setting)
        {
            ZoneSettings = new ObservableCollection<ZoneSettingViewModel>();
            var limits = setting.Limits.ToList();
            for (int i = 0; i < limits.Count; i++)
            {
                ZoneSettings.Add(new ZoneSettingViewModel
                {
                    ZoneName = $"Zone {i + 1}",
                    ZoneValue = limits[i]
                });
            }

            Name = setting.Name;
        }

        public ObservableCollection<ZoneSettingViewModel> ZoneSettings { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (!value.Equals(_name))
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
