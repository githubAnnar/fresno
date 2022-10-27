using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using System;
using System.Security;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class EmailPreferencesViewModel : ViewModelBase
    {
        #region Fields

        private string _server;
        private string _from;
        private string _displayName;
        private SecureString _password;
        private ICommand _saveCommand;

        #endregion

        #region Constructors

        public EmailPreferencesViewModel()
        {
            Server = ApplicationSettingsManager.EmailServer;
            From = ApplicationSettingsManager.EmailFrom;
            EmailDisplayName = ApplicationSettingsManager.EmailDisplayName;
            Port = ApplicationSettingsManager.Port;
            Username = ApplicationSettingsManager.Username;
            Password = PasswordHelpers.DecryptString(ApplicationSettingsManager.Password);
        }

        #endregion

        #region Properties

        public string Server
        {
            get => _server;
            set
            {
                if (string.IsNullOrEmpty(_server) || !_server.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _server = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Port { get; set; }

        public string Username { get; set; }

        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password == null || !_password.Equals(value))
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string From
        {
            get => _from;
            set
            {
                if (string.IsNullOrEmpty(_from) || !_from.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _from = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EmailDisplayName
        {
            get => _displayName;
            set
            {
                if (string.IsNullOrEmpty(_displayName) || !_displayName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save()));

        #region Methods

        public void Save()
        {
            if (!ApplicationSettingsManager.EmailServer.Equals(Server, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailServer = Server;
            }

            if (ApplicationSettingsManager.Port != Port)
            {
                ApplicationSettingsManager.Port = Port;
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.EmailFrom) || !ApplicationSettingsManager.EmailFrom.Equals(From, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailFrom = From;
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.EmailDisplayName) || !ApplicationSettingsManager.EmailDisplayName.Equals(EmailDisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailDisplayName = EmailDisplayName;
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.Username) || !ApplicationSettingsManager.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.Username = Username;
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.Password) || !ApplicationSettingsManager.Password.Equals(PasswordHelpers.EncryptString(Password)))
            {
                ApplicationSettingsManager.Password = PasswordHelpers.EncryptString(Password);
            }
        }

        #endregion
    }
}
