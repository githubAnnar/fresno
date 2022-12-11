using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
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
        }

        #endregion

        #region Commands

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(param => Close()));

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save()));

        #endregion

        #region Methods

        public Action Close { get; }

        private void Save() { }

        #endregion
    }
}
