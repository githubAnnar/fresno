using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services.Settings;
using LanterneRouge.Fresno.WpfClient.UserControls.ViewModel;
using LanterneRouge.Wpf.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class LactateZonePreferencesViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _closeCommand;
        private ICommand _saveCommand;
        private ICommand _resetCommand;

        #endregion

        #region Constructors

        public LactateZonePreferencesViewModel(Action close)
        {
            Close = close;

            var savedPrefs = ApplicationSettingsManager.ZoneSettingsValue;
            ZonePrefs = new ObservableCollection<ZoneSettingsViewModel>();
            if (savedPrefs == null)
            {
                foreach (var item in ZoneSettings.Default)
                {
                    ZonePrefs.Add(new ZoneSettingsViewModel(item));
                }
            }

            else
            {
                foreach (var item in savedPrefs)
                {
                    ZonePrefs.Add(new ZoneSettingsViewModel(item));
                }
            }
        }

        #endregion

        #region Commands

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(param => Close()));

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save()));

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand(param => Reset()));

        #endregion

        #region Methods

        public ObservableCollection<ZoneSettingsViewModel> ZonePrefs { get; set; }

        public Action Close { get; }

        private void Save()
        {
            var saveSettings = new ZoneSettings();
            foreach (var item in ZonePrefs)
            {
                var newLimits = new List<double>();
                foreach (var limit in item.ZoneSettings)
                {
                    newLimits.Add(limit.ZoneValue);
                }

                var saveItem = new ZoneSetting
                {
                    Name = item.Name,
                    Limits = newLimits
                };

                saveSettings.Add(saveItem);
            }

            ApplicationSettingsManager.ZoneSettingsValue = saveSettings;

            Close();
        }

        private void Reset()
        {
            ZonePrefs.Clear();
            foreach (var item in ZoneSettings.Default)
            {
                ZonePrefs.Add(new ZoneSettingsViewModel(item));
            }
        }

        #endregion
    }
}
