using LanterneRouge.Fresno.WpfClient.Preferences.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace LanterneRouge.Fresno.WpfClient.Preferences.View
{
    /// <summary>
    /// Interaction logic for EmailPreferencesView.xaml
    /// </summary>
    public partial class EmailPreferencesView : UserControl
    {
        public EmailPreferencesView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb && DataContext is EmailPreferencesViewModel dm)
            {
                dm.Password = pb.SecurePassword;
            }
        }
    }
}
