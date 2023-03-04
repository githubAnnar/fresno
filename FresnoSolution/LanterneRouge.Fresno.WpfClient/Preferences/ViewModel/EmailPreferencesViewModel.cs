using LanterneRouge.Fresno.Utils.Helpers;
using LanterneRouge.Fresno.WpfClient.MVVM;
using log4net;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class EmailPreferencesViewModel : ViewModelBase
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmailPreferencesViewModel));
        private string _server;
        private string _from;
        private string _displayName;
        private int _port;
        private string _userName;
        private SecureString _password;
        private ICommand _saveCommand;
        private ICommand _closeCommand;

        #endregion

        #region Constructors

        public EmailPreferencesViewModel(Action close)
        {
            Server = ApplicationSettingsManager.EmailServer;
            From = ApplicationSettingsManager.EmailFrom;
            EmailDisplayName = ApplicationSettingsManager.EmailDisplayName;
            Port = ApplicationSettingsManager.Port;
            Username = ApplicationSettingsManager.Username;
            Password = PasswordHelpers.DecryptString(ApplicationSettingsManager.Password);
            Close = close;
        }

        #endregion

        #region Properties

        public Action Close { get; }

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

        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _userName;
            set
            {
                if (string.IsNullOrEmpty(_userName) || !_userName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(param => Save());

        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(param => Close());

        #region Methods

        public void Save()
        {
            var propList = new List<string>();
            if (string.IsNullOrEmpty(ApplicationSettingsManager.EmailServer) || !ApplicationSettingsManager.EmailServer.Equals(Server, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailServer = Server;
                propList.Add($"{nameof(ApplicationSettingsManager.EmailServer)}='{ApplicationSettingsManager.EmailServer}'");
            }

            if (ApplicationSettingsManager.Port != Port)
            {
                ApplicationSettingsManager.Port = Port;
                propList.Add($"{nameof(ApplicationSettingsManager.Port)}='{ApplicationSettingsManager.Port}'");
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.EmailFrom) || !ApplicationSettingsManager.EmailFrom.Equals(From, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailFrom = From;
                propList.Add($"{nameof(ApplicationSettingsManager.EmailFrom)}='{ApplicationSettingsManager.EmailFrom}'");
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.EmailDisplayName) || !ApplicationSettingsManager.EmailDisplayName.Equals(EmailDisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.EmailDisplayName = EmailDisplayName;
                propList.Add($"{nameof(ApplicationSettingsManager.EmailDisplayName)}='{ApplicationSettingsManager.EmailDisplayName}'");
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.Username) || !ApplicationSettingsManager.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase))
            {
                ApplicationSettingsManager.Username = Username;
                propList.Add($"{nameof(ApplicationSettingsManager.Username)}='{ApplicationSettingsManager.Username}'");
            }

            if (string.IsNullOrEmpty(ApplicationSettingsManager.Password) || !ApplicationSettingsManager.Password.Equals(PasswordHelpers.EncryptString(Password)))
            {
                ApplicationSettingsManager.Password = PasswordHelpers.EncryptString(Password);
                propList.Add(nameof(ApplicationSettingsManager.Password));
            }

            Logger.Info($"Saved values: {string.Join(", ", propList)}");
            Close();
        }

        #endregion
    }
}
