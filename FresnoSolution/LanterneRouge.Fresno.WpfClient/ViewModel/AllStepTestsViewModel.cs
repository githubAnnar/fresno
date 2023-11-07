using Autofac;
using LanterneRouge.Fresno.Report;
using LanterneRouge.Fresno.Services;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.Utils;
using LanterneRouge.Wpf.MVVM;
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
using System.Text;
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

        public AllStepTestsViewModel(UserViewModel parentUser, MainWindowViewModel rootViewModel) : base(parentUser, rootViewModel, new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-diabetes-96.png")))
        {
            DisplayName = parentUser == null ? "All Users"/*KayakStrings.Category_All_Categories*/ : $"StepTests: {parentUser.LastName}";
            CreateAllStepTests();
            //DataManager.Committed += DataManager_Committed;
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
                new CommandViewModel("FBLC Calculation", ShowFblcCalculationCommand, "Fixed Blood Lactate Consentration Calculation"),
                new CommandViewModel("FRPB Calculation", ShowFrpbCalculationCommand,"Fixed Rise Post Baseline Calculation"),
                new CommandViewModel("LT Calculation", ShowLtCalculationCommand,"Lactate Threshold Calculation"),
                new CommandViewModel("LT Log Calculation", ShowLtLogCalculationCommand),
                new CommandViewModel("DMax Calculation", ShowDMaxCalculationCommand),
            };
        }

        private void DataManager_Committed()
        {
            OnDispose();
            CreateAllStepTests();
        }

        private async void CreateAllStepTests()
        {
            if (Parent is UserViewModel parent)
            {
                var stepTests = await DataManager.GetAllStepTestsByUser(parent.Source);
                var all = stepTests.Select(s => new StepTestViewModel(s, parent, RootViewModel)).ToList();
                all.ForEach(cvm => cvm.PropertyChanged += OnStepTestViewModelPropertyChanged);
                AllStepTests = new ObservableCollection<StepTestViewModel>(all);
                OnPropertyChanged(nameof(AllStepTests));
                AllStepTests.CollectionChanged += OnCollectionChanged;
                Logger.Debug("AllStepTests created");
            }
        }

        #endregion

        #region Public Interface

        public StepTestViewModel Selected => SelectedObject as StepTestViewModel;

        public int SelectedMeasurementCount => Selected != null ? DataManager.GetMeasurementCountByStepTest(Selected.Source, true).Result : 0;

        public ObservableCollection<StepTestViewModel> AllStepTests { get; private set; }

        public IEmailService EmailService
        {
            get
            {
                if (_emailService == null)
                {
                    var scope = ServiceLocator.Instance.BeginLifetimeScope();
                    _emailService = scope.Resolve<IEmailService>();
                    _emailService.MailIsSentCallback = SendCompleteCallback;
                }

                return _emailService;
            }
        }

        private void SendCompleteCallback(object sender, AsyncCompletedEventArgs e)
        {
            var message = e.UserState as MailMessage;

            if (e.Cancelled)
            {
                var cancelledMessage = new StringBuilder();
                if (message != null)
                {
                    cancelledMessage.Append($"Send Message to {string.Join(", ", message.To.Select(m => m.Address))} is cancelled!");
                }

                else
                {
                    cancelledMessage.Append("Send Message is cancelled!");
                }

                Logger.Warn(cancelledMessage.ToString());
                MessageBox.Show(cancelledMessage.ToString(), "Sending Message Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (e.Error != null)
            {
                var errorMessage = new StringBuilder();
                if (message != null)
                {
                    errorMessage.Append($"Send Message to {string.Join(", ", message.To.Select(m => m.Address))} failed!");
                }

                else
                {
                    errorMessage.Append("Send Message failed!");
                }

                Logger.Error(errorMessage.ToString(), e.Error);
                MessageBox.Show(errorMessage.ToString(), "Error Sending Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                var sentMessage = new StringBuilder();
                if (message != null)
                {
                    sentMessage.Append($"Message is sent to {string.Join(", ", message.To.Select(m => m.Address))}");
                }

                else
                {
                    sentMessage.Append("Message is sent");
                }

                Logger.Info(sentMessage.ToString());
                MessageBox.Show(sentMessage.ToString(), "Message Sent", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            message?.Dispose();
        }

        #region ShowDiagram Command

        public ICommand ShowDiagramCommand => _showDiagramCommand ??= new RelayCommand(ShowDiagram, param => AllStepTests.Any(at => at.IsSelected) && AllSelected.Cast<StepTestViewModel>().All(s => DataManager.GetMeasurementCountByStepTest(s.Source, true).Result > 3));

        private void ShowDiagram(object obj) => new StepTestsPlotViewModel(AllStepTests.Where(st => st.IsSelected), RootViewModel).Show();

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
        //public override WorkspaceViewModel SelectedObject => AllStepTests.FirstOrDefault(item => item.IsSelected);

        public override WorkspaceViewModel SelectedObject
        {
            get
            {
                if (AllStepTests.Any(item => item.IsSelected))
                {
                    return AllStepTests.First(item => item.IsSelected);
                }

                AllStepTests.Last().IsSelected = true;
                return AllStepTests.Last();
            }
        }

        public IEnumerable<WorkspaceViewModel> AllSelected => AllStepTests.Where(item => item.IsSelected);

        public static string GetIdentifierName(StepTestViewModel stepTest) => $"{Name}_StepTest_{stepTest.StepTestId}";

        public ICommand AddStepTestCommand => _addStepTestCommand ??= new RelayCommand(param => CreateChild());

        public override void CreateChild() => StepTestViewModel.Create(Parent as UserViewModel, RootViewModel);

        public ICommand ShowUserCommand => _showUserCommand ??= new RelayCommand(param => Parent.Show(), param => Parent != null);

        public ICommand ShowAllMeasurementsCommand => _showAllMeasurementsCommand ??= new RelayCommand(param => ShowAllMeasurements(), param => Selected != null && Selected.IsValid);

        public ICommand CreateStepTestPdfCommand => _createStepTestPdfCommand ??= new RelayCommand(param => CreateStepTestPdf(), param => AllSelected.Any() && AllSelected.Cast<StepTestViewModel>().All(s => DataManager.GetMeasurementCountByStepTest(s.Source, true).Result > 3));

        private void CreateStepTestPdf()
        {
            var filename = GeneratePdf();
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
            var generator = new StepTestReport(main.Source, rest.Select(item => item.Source).ToList());
            var pdfDocument = generator.CreateReport();
            var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{parent.FirstName} {parent.LastName} ({main.Source.Id}).pdf");
            generator.PdfRender(filename, pdfDocument);
            return filename;
        }

        public ICommand SendPdfByEmailCommand => _sendPdfByEmailCommand ??= new RelayCommand(param => SendStepTestPdfByMail(), param => CanSendEmail);

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
                EmailService.SendEmail(message);
            }
        }

        public bool CanSendEmail => AllSelected.Count() == 1 && !string.IsNullOrEmpty((Parent as UserViewModel)?.Email) && ApplicationSettingsManager.IsEmailSettingsValid();

        public ICommand ShowFblcCalculationCommand => _showFblcCalculationCommand ??= new RelayCommand(Selected.ShowFblcCalculationCommand.Execute, param => AllSelected.Count() == 1 && SelectedMeasurementCount > 3);

        public ICommand ShowFrpbCalculationCommand => _showFrpbCalculationCommand ??= new RelayCommand(Selected.ShowFrpbCalculationCommand.Execute, param => AllSelected.Count() == 1 && SelectedMeasurementCount > 3);

        public ICommand ShowLtCalculationCommand => _showLtCalculationCommand ??= new RelayCommand(Selected.ShowLtCalculationCommand.Execute, param => AllSelected.Count() == 1 && SelectedMeasurementCount > 4);

        public ICommand ShowLtLogCalculationCommand => _showLtLogCalculationCommand ??= new RelayCommand(Selected.ShowLtLogCalculationCommand.Execute, param => AllSelected.Count() == 1 && SelectedMeasurementCount > 4);

        public ICommand ShowDMaxCalculationCommand => _showDMaxCalculationCommand ??= new RelayCommand(Selected.ShowDMaxCalculationCommand.Execute, param => AllSelected.Count() == 1 && SelectedMeasurementCount > 3);

        private void ShowAllMeasurements()
        {
            var workspace = new AllMeasurementsViewModel(Selected, RootViewModel);
            workspace.Show();
        }

        public bool Equals(AllStepTestsViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is AllStepTestsViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((UserViewModel)Parent).GetHashCode();
    }
}
