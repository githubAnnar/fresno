using LanterneRouge.Fresno.WpfClient.ViewModel;
using System.ComponentModel;

namespace LanterneRouge.Fresno.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.SaveMru();
            }
        }
    }
}
