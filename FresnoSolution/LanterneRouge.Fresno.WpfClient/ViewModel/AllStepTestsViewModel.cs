using Autofac;
using LanterneRouge.Fresno.Report;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllStepTestsViewModel : WorkspaceViewModel, IEquatable<AllStepTestsViewModel>
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AllStepTestsViewModel));
        private static readonly string Name = typeof(AllStepTestsViewModel).Name;
        private ICommand _showDiagramCommand;
        private ICommand _addStepTestCommand;
        private ICommand _showUserCommand;
        private ICommand _showAllMeasurementsCommand;
        private ICommand _createStepTestPdfCommand;
        private ICommand _showFblcCalculationCommand;
        private ICommand _showFrpbCalculationCommand;
        private ICommand _showLtCalculationCommand;
        private ICommand _showLtLogCalculationCommand;
        private ICommand _showDMaxCalculationCommand;
        private ICommand _sendPdfByEmailCommand;
        private IEmailService _emailService;

        #endregion

        #region Constructors

        public AllStepTestsViewModel(UserViewModel parentUser, Action<WorkspaceViewModel> showWorkspace) : base(parentUser, showWorkspace, new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-diabetes-96.png")))
        {
            DisplayName = parentUser == null ? "All Users"/*KayakStrings.Category_All_Categories*/ : $"StepTests: {parentUser.LastName}";
            CreateAllStepTests();
            DataManager.Committed += DataManager_Committed;
            Logger.Debug($"AllStepests for user {parentUser.LastName} loaded");

            // Set up sub commands
            SubCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Add Steptest", AddStepTestCommand),
                new CommandViewModel("Show User", ShowUserCommand),
                new CommandViewModel("Show all Measurements", ShowAllMeasurementsCommand),
                new CommandViewModel("Show Diagram", ShowDiagramCommand),
                new CommandViewModel("Generate PDF", CreateStepTestPdfCommand),
                new CommandViewModel("Send PDF by Email", SendPdfByEmailCommand),
                new CommandViewModel("FBLC Calculation", ShowFblcCalculationCommand),
                new CommandViewModel("FRPB Calculation", ShowFrpbCalculationCommand),
                new CommandViewModel("LT Calculation", ShowLtCalculationCommand),
                new CommandViewModel("LT Log Calculation", ShowLtLogCalculationCommand),
                new CommandViewModel("DMax Calculation", ShowDMaxCalculationCommand),
            };
        }

        private void DataManager_Committed()
        {
            OnDispose();
            CreateAllStepTests();
        }

        private void CreateAllStepTests()
        {
            if (Parent is UserViewModel parent)
            {
                var all = (from stepTest in parent.Source.StepTests select new StepTestViewModel(stepTest, parent, ShowWorkspace)).ToList();
                all.ForEach(cvm => cvm.PropertyChanged += OnStepTestViewModelPropertyChanged);
                AllStepTests = new ObservableCollection<StepTestViewModel>(all);
                AllStepTests.CollectionChanged += OnCollectionChanged;
            }
        }

        #endregion

        #region Public Interface

        public StepTestViewModel Selected => SelectedObject as StepTestViewModel;

        public ObservableCollection<StepTestViewModel> AllStepTests { get; private set; }

        public IEmailService EmailService
        {
            get
            {
                if (_emailService == null)
                {
                    var scope = ServiceLocator.Instance.BeginLifetimeScope();
                    _emailService = scope.Resolve<IEmailService>();
                }

                return _emailService;
            }
        }



        #region ShowDiagram Command

        public ICommand ShowDiagramCommand => _showDiagramCommand ?? (_showDiagramCommand = new RelayCommand(ShowDiagram, param => AllStepTests.Any(at => at.IsSelected)));

        private void ShowDiagram(object obj) => new StepTestsPlotViewModel(AllStepTests.Where(st => st.IsSelected), ShowWorkspace).Show();

        #endregion

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// Child classes can override this method to perform
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected override void OnDispose()
        {
            foreach (var stepTestVM in AllStepTests)
            {
                Logger.Debug($"Disposing {stepTestVM.DisplayName}");
                stepTestVM.Dispose();
            }

            AllStepTests.Clear();
            AllStepTests.CollectionChanged -= OnCollectionChanged;
        }

        #endregion

        #region Event Handling Methods

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && !e.NewItems.Count.Equals(0))
            {
                foreach (StepTestViewModel stepTestVM in e.NewItems)
                {
                    stepTestVM.PropertyChanged += OnStepTestViewModelPropertyChanged;
                    Logger.Debug($"New StepTestViewModel {stepTestVM.DisplayName}");
                }
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (StepTestViewModel stepTestVM in e.OldItems)
                {
                    stepTestVM.PropertyChanged -= OnStepTestViewModelPropertyChanged;
                    Logger.Debug($"Old StepTestViewModel {stepTestVM.DisplayName}");
                }
            }
        }

        private void OnStepTestViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string isSelected = "IsSelected";

            if (sender is StepTestViewModel stepTestViewModel)
            {
                stepTestViewModel.VerifyPropertyName(isSelected);
            }
        }

        #endregion

        /// <summary>
        /// Gets the selected object.
        /// </summary>
        public override WorkspaceViewModel SelectedObject => AllStepTests.FirstOrDefault(item => item.IsSelected);

        public IEnumerable<WorkspaceViewModel> AllSelected => AllStepTests.Where(item => item.IsSelected);

        public static string GetIdentifierName(StepTestViewModel stepTest) => $"{Name}_StepTest_{stepTest.StepTestId}";

        public ICommand AddStepTestCommand => _addStepTestCommand ?? (_addStepTestCommand = new RelayCommand(param => CreateChild()));

        public override void CreateChild() => StepTestViewModel.Create(Parent as UserViewModel, ShowWorkspace);

        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(param => Parent.Show(), param => Parent != null));

        public ICommand ShowAllMeasurementsCommand => _showAllMeasurementsCommand ?? (_showAllMeasurementsCommand = new RelayCommand(param => ShowAllMeasurements(), param => Selected != null && Selected.IsValid));

        public ICommand CreateStepTestPdfCommand => _createStepTestPdfCommand ?? (_createStepTestPdfCommand = new RelayCommand(param => CreateStepTestPdf(), param => AllSelected.Count() >= 1));

        private void CreateStepTestPdf()
        {
            string filename = GeneratePdf();

            MessageBox.Show($"PDF {filename} is generated", "PDF Generation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GeneratePdf()
        {
            var allSelectedStepTests = AllSelected.Cast<StepTestViewModel>();

            // Find newest steptest
            var newestDate = allSelectedStepTests.Select(item => item.Source.TestDate).Max();
            var main = allSelectedStepTests.First(item => item.Source.TestDate.Equals(newestDate));

            // Find the rest selected ones
            var rest = AllSelected.Cast<StepTestViewModel>().Where(item => !item.Source.TestDate.Equals(newestDate)).ToList();
            var parent = Parent as UserViewModel;
            var generator = new StepTestReport(main.Source, rest.Select(item => item.Source));
            var pdfDocument = generator.CreateReport();
            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{parent.FirstName} {parent.LastName} ({main.Source.Id}).pdf");
            generator.PdfRender(filename, pdfDocument);
            return filename;
        }

        public ICommand SendPdfByEmailCommand => _sendPdfByEmailCommand ?? (_sendPdfByEmailCommand = new RelayCommand(param => SendStepTestPdfByMail(), param => CanSendEmail));

        private void SendStepTestPdfByMail()
        {
            if (Parent is UserViewModel user)
            {
                var filename = GeneratePdf();

                // Generate mail message
                var senderEmail = new MailAddress(ApplicationSettingsManager.EmailFrom, !string.IsNullOrEmpty(ApplicationSettingsManager.EmailDisplayName) ? ApplicationSettingsManager.EmailDisplayName : null);

                var message = new MailMessage(senderEmail, new MailAddress(user.Email))
                {
                    Subject = $"Lactate Steptest result for {user.FirstName} {user.LastName}",
                    Body = "The results of the test is attached to the mail",
                    Sender = senderEmail
                };

                message.CC.Add(senderEmail);
                var fileData = new Attachment(filename, MediaTypeNames.Application.Pdf);
                var disposition = fileData.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(filename);
                disposition.ModificationDate = File.GetLastWriteTime(filename);
                disposition.ReadDate = File.GetLastAccessTime(filename);

                message.Attachments.Add(fileData);

                // Send it by mail
                if (EmailService.SendEmail(message))
                {

                }
            }

        }

        public bool CanSendEmail => AllSelected.Count() == 1 && !string.IsNullOrEmpty((Parent as UserViewModel)?.Email) && ApplicationSettingsManager.IsEmailSettingsValid();

        public ICommand ShowFblcCalculationCommand => _showFblcCalculationCommand ?? (_showFblcCalculationCommand = new RelayCommand((object obj) => { Selected.ShowFblcCalculationCommand.Execute(obj); }, param => AllSelected.Count() == 1));

        public ICommand ShowFrpbCalculationCommand => _showFrpbCalculationCommand ?? (_showFrpbCalculationCommand = new RelayCommand((object obj) => { Selected.ShowFrpbCalculationCommand.Execute(obj); }, param => AllSelected.Count() == 1));

        public ICommand ShowLtCalculationCommand => _showLtCalculationCommand ?? (_showLtCalculationCommand = new RelayCommand((object obj) => { Selected.ShowLtCalculationCommand.Execute(obj); }, param => AllSelected.Count() == 1));

        public ICommand ShowLtLogCalculationCommand => _showLtLogCalculationCommand ?? (_showLtLogCalculationCommand = new RelayCommand((object obj) => { Selected.ShowLtLogCalculationCommand.Execute(obj); }, param => AllSelected.Count() == 1));

        public ICommand ShowDMaxCalculationCommand => _showDMaxCalculationCommand ?? (_showDMaxCalculationCommand = new RelayCommand((object obj) => { Selected.ShowDMaxCalculationCommand.Execute(obj); }, param => AllSelected.Count() == 1));

        private void ShowAllMeasurements()
        {
            var workspace = new AllMeasurementsViewModel(Selected, ShowWorkspace);
            workspace.Show();
        }

        public bool Equals(AllStepTestsViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is AllStepTestsViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((UserViewModel)Parent).GetHashCode();
    }
}
