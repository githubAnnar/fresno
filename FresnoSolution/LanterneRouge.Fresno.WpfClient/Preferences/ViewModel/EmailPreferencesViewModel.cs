using LanterneRouge.Fresno.WpfClient.MVVM;
using System;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class EmailPreferencesViewModel : ViewModelBase
    {
        #region Fields

        private string _server;
        private string _from;
        private string _displayName;


        #endregion

        #region Constructors

        public EmailPreferencesViewModel()
        {
            Server = Properties.Settings.Default.EmailServer;
            From = Properties.Settings.Default.EmailFrom;
            EmailDisplayName = Properties.Settings.Default.EmailDisplayName;
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

        public string From
        {
            get => _from; set
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
            get => _displayName; set
            {
                if (string.IsNullOrEmpty(_displayName) || !_displayName.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        public void Save()
        {
            if (!Properties.Settings.Default.EmailServer.Equals(Server, StringComparison.InvariantCultureIgnoreCase))
            {
                Properties.Settings.Default.EmailServer = Server;
            }

            if (!Properties.Settings.Default.EmailFrom.Equals(From, StringComparison.InvariantCultureIgnoreCase))
            {
                Properties.Settings.Default.EmailFrom = From;
            }

            if (!Properties.Settings.Default.EmailDisplayName.Equals(EmailDisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                Properties.Settings.Default.EmailDisplayName = EmailDisplayName;
            }
        }

        #endregion
    }
}
