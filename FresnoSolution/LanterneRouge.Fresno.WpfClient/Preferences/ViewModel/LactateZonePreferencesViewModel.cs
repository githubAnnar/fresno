using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.UserControls.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class LactateZonePreferencesViewModel
    {
        #region Fields

        private ICommand _closeCommand;
        private ICommand _saveCommand;

        #endregion

        #region Constructors

        public LactateZonePreferencesViewModel(Action close)
        {
            Close = close;
            ZonePrefs = new ObservableCollection<ZoneSettingsViewModel>();
            foreach (var item in ZoneSettings.Default)
            {
                ZonePrefs.Add(new ZoneSettingsViewModel(item));
            }
        }

        #endregion

        #region Commands

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(param => Close()));

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save()));

        #endregion

        #region Methods

        public ObservableCollection<ZoneSettingsViewModel> ZonePrefs { get; set; }

        public Action Close { get; }

        private void Save() { }

        #endregion
    }
}
