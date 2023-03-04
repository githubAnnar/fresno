using LanterneRouge.Fresno.WpfClient.ViewModel;
using log4net;
using System.ComponentModel;

namespace LanterneRouge.Fresno.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                Logger.Info("Saving MRU");
                vm.SaveMru();
                Logger.Info("MRU Saved, Closing application");
                Logger.Info("**** ---- END ---- ****");
            }
        }
    }
}
