using Bluegrams.Application;
using LanterneRouge.Fresno.WpfClient.ViewModel;
using System;
using System.Windows;

namespace LanterneRouge.Fresno.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var serviceLocator = ServiceLocator.Default;
            //serviceLocator.RegisterType<IDataService, DataService>();

            //var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            //viewModelLocator.Register(typeof(MainWindow), typeof(MainWindowViewModel));

            //var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            //uiVisualizerService.Register(typeof(UserViewModel), typeof(UserView));
            //uiVisualizerService.Register(typeof(StepTestViewModel), typeof(StepTestView));
            //uiVisualizerService.Register(typeof(MeasurementViewModel), typeof(MeasurementView));

            base.OnStartup(e);
            PortableJsonSettingsProvider.ApplyProvider(WpfClient.Properties.Settings.Default);
            var window = new MainWindow();
            var viewModel = new MainWindowViewModel(window);

            void handler(object sender, EventArgs ea)
            {
                viewModel.RequestClose -= handler;
                window.Close();
            }

            viewModel.RequestClose += handler;
            window.DataContext = viewModel;
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //// Get advisory report in console
            //ApiCopManager.AddListener(new ConsoleApiCopListener());
            //ApiCopManager.WriteResults();

            base.OnExit(e);
        }
    }
}
