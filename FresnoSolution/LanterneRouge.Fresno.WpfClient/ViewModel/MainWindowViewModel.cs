using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.Report;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.View;
using Microsoft.Win32;
using MRULib;
using MRULib.MRU.Interfaces;
using MRULib.MRU.Models.Persist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class MainWindowViewModel : WorkspaceViewModel, IWorkspaceCommands
    {
        #region Fields

        private ReadOnlyCollection<CommandViewModel> _commands;
        private ObservableCollection<WorkspaceViewModel> _workspaces;
        private ICommand _openFileCommand;
        private ICommand _newFileCommand;
        private ICommand _navigateUriCommand;
        private readonly string _mruPersistPath;
        private bool _isProcessingMru;

        private const string AppDirecory = "LanterneRouge";
        private const string MruFilename = "FresnoMru.xml";

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DisplayName = $"Step Test Viewer";

            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppDirecory}"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppDirecory}");
            }

            _mruPersistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppDirecory}\\{MruFilename}";
            if (!File.Exists(_mruPersistPath))
            {
                MRUFileList = MRU_Service.Create_List();
            }

            else
            {
                LoadMru(_mruPersistPath);
            }
        }

        #endregion // Constructor

        #region Properties

        public IMRUListViewModel MRUFileList { get; private set; }

        public bool IsProcessingMru
        {
            get => _isProcessingMru;

            private set
            {
                if (_isProcessingMru != value)
                {
                    _isProcessingMru = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands => _commands ?? (_commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands()));

        /// <summary>
        /// Creates the commands.
        /// </summary>
        /// <returns></returns>
        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("All Users", new RelayCommand(param => ShowAllUsers(), param=>IsDatabaseOpen)),
                new CommandViewModel($"Create New User", new RelayCommand(param => CreateNewUser(), param=>IsDatabaseOpen)),
                new CommandViewModel("All Step Tests", new RelayCommand(param => ShowAllStepTests(param as UserViewModel), param=>CanShowAllStepTests)),
                new CommandViewModel($"Create New Step Test", new RelayCommand(param => CreateNewStepTest(param), param => CanCreateStepTest)),
                new CommandViewModel("Generate PDF Report", new RelayCommand(param => GenerateStepTestPdf(param as StepTestViewModel), param => CanGenerateStepTestPdf)),
                new CommandViewModel("All Measurements", new RelayCommand(param=> ShowAllMeasurements(param as StepTestViewModel), param => CanShowAllMeasurements)),
                new CommandViewModel($"Create New Measurement", new RelayCommand(param=>CreateNewMeasurement(param), param=>CanCreateMeasurement))
            };
        }

        #region OpenFileCommand

        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new RelayCommand(param => OpenFile()));

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "sqlite",
                Filter = "SqLite database files (*.sqlite)|*.sqlite|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            var openFileResult = openFileDialog.ShowDialog();
            if (openFileResult.HasValue && openFileResult.Value)
            {
                CurrentDatabaseFilename = openFileDialog.FileName;
                if (ServiceLocator.Instance.GetService(typeof(IDataService)) is DataService service)
                {
                    IsDatabaseOpen = service.LoadDatabase(CurrentDatabaseFilename);
                    MRUFileList.UpdateEntry(CurrentDatabaseFilename);
                    ShowAllUsers();
                }
            }
        }

        #endregion

        #region NewFileCommand

        public ICommand NewFileCommand => _newFileCommand ?? (_newFileCommand = new RelayCommand(param => NewFile()));

        private void NewFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                AddExtension = true,
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = "sqlite",
                Filter = "SqLite database files (*.sqlite)|*.sqlite|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            var openFileResult = openFileDialog.ShowDialog();
            if (openFileResult.HasValue && openFileResult.Value)
            {
                CurrentDatabaseFilename = openFileDialog.FileName;
                if (ServiceLocator.Instance.GetService(typeof(IDataService)) is DataService service)
                {
                    IsDatabaseOpen = service.LoadDatabase(CurrentDatabaseFilename);
                    MRUFileList.UpdateEntry(CurrentDatabaseFilename);
                    ShowAllUsers();
                }
            }
        }

        #endregion

        #region NavigateUriCommand

        public ICommand NavigateUriCommand => _navigateUriCommand ?? (_navigateUriCommand = new RelayCommand((object param) =>
        {
            if (!(param is string path))
            {
                return;
            }

            // If file does not exist, end here
            if (!File.Exists(path))
            {
                MessageBox.Show($"The file '{path}' does not exist anymore, please create a new database!", "File not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                MRUFileList.RemoveEntry(path);
                return;
            }

            if (ServiceLocator.Instance.GetService(typeof(IDataService)) is DataService service)
            {
                IsDatabaseOpen = service.LoadDatabase(path);
                MRUFileList.UpdateEntry(path);
                ShowAllUsers();
            }
        }));

        #endregion

        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }

                return _workspaces;
            }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += OnWorkspaceRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= OnWorkspaceRequestClose;
                }
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            if (!(sender is WorkspaceViewModel workspace)) return;
            workspace.Dispose();
            Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers

        #region User Helpers

        /// <summary>
        /// Creates the new user.
        /// </summary>
        private void CreateNewUser()
        {
            var newUser = User.Create(string.Empty, string.Empty, null, null, null, DateTime.Now, 0, 0, "M", null);
            newUser.AcceptChanges();
            var workspace = new UserViewModel(newUser, this);
            ShowWorkspace(workspace);
        }

        public void ShowUser(UserViewModel user)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is UserViewModel && (vm as UserViewModel).UserId.Equals(user.UserId)) is UserViewModel workspace))
            {
                workspace = user;
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowUser(StepTestViewModel stepTest)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is UserViewModel && (vm as UserViewModel).UserId.Equals(stepTest.Source.ParentUser.Id)) is UserViewModel workspace))
            {
                workspace = new UserViewModel(stepTest.Source.ParentUser, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowUser(MeasurementViewModel measurement)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is UserViewModel && (vm as UserViewModel).UserId.Equals(measurement.Source.ParentStepTest.ParentUser.Id)) is UserViewModel workspace))
            {
                workspace = new UserViewModel(measurement.Source.ParentStepTest.ParentUser, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        private void ShowAllUsers()
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is AllUsersViewModel) is AllUsersViewModel workspace))
            {
                workspace = new AllUsersViewModel(this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        #endregion

        #region StepTest Helpers

        /// <summary>
        /// Creates the new step test.
        /// </summary>
        /// <param name="stepTest">The race object.</param>
        public void CreateNewStepTest(object userObject)
        {
            // First we must have a race view model object
            UserViewModel user = null;

            // Check parameter
            if (userObject is UserViewModel)
            {
                user = userObject as UserViewModel;
            }

            // Try to get it from current workspace
            else
            {
                var wvm = GetActiveWorkspace();
                if (wvm is AllUsersViewModel avm)
                {
                    user = avm.AllUsers.FirstOrDefault(item => item.IsSelected);
                }

                else if (wvm is UserViewModel uvm)
                {
                    user = uvm;
                }
            }

            var newStepTest = StepTest.Create(user.UserId, "Bike", "W", TimeSpan.FromMinutes(4d).Ticks, 0, 0, 0, 0, DateTime.Now);
            newStepTest.ParentUser = user.Source;
            user.Source.StepTests.Add(newStepTest);
            newStepTest.AcceptChanges();
            var workspace = new StepTestViewModel(newStepTest, this);
            ShowWorkspace(workspace);
        }

        public void ShowFblcCalculation(StepTestViewModel stepTestVm)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is FblcCalculationViewModel && (vm as FblcCalculationViewModel).StepTestId.Equals(stepTestVm.StepTestId)) is FblcCalculationViewModel workspace))
            {
                workspace = new FblcCalculationViewModel(stepTestVm.Source, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowFrpbCalculation(StepTestViewModel stepTestVm)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is FrpbCalculationViewModel && (vm as FrpbCalculationViewModel).StepTestId.Equals(stepTestVm.StepTestId)) is FrpbCalculationViewModel workspace))
            {
                workspace = new FrpbCalculationViewModel(stepTestVm.Source, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowLtCalculation(StepTestViewModel stepTestVm)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is LtCalculationViewModel && (vm as LtCalculationViewModel).StepTestId.Equals(stepTestVm.StepTestId)) is LtCalculationViewModel workspace))
            {
                workspace = new LtCalculationViewModel(stepTestVm.Source, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowLtLogCalculation(StepTestViewModel stepTestVm)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is LtLogCalculationViewModel && (vm as LtLogCalculationViewModel).StepTestId.Equals(stepTestVm.StepTestId)) is LtLogCalculationViewModel workspace))
            {
                workspace = new LtLogCalculationViewModel(stepTestVm.Source, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowStepTest(StepTestViewModel stepTest)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is StepTestViewModel && (vm as StepTestViewModel).StepTestId.Equals(stepTest.StepTestId)) is StepTestViewModel workspace))
            {
                workspace = stepTest;
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowStepTest(MeasurementViewModel measurement)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is StepTestViewModel && (vm as StepTestViewModel).StepTestId.Equals(measurement.Source.ParentStepTest.Id)) is StepTestViewModel workspace))
            {
                workspace = new StepTestViewModel(measurement.Source.ParentStepTest, this);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowAllStepTests(UserViewModel user)
        {
            if (user == null)
            {
                user = GetActiveSelectedObject() as UserViewModel;
            }

            var workspace = new AllStepTestsViewModel(user, this);
            ShowWorkspace(workspace);
        }

        public bool CanShowAllStepTests
        {
            get
            {
                WorkspaceViewModel wvm;

                if ((wvm = GetActiveSelectedObject()) == null)
                {
                    return false;
                }

                return wvm is UserViewModel;
            }
        }

        public bool CanCreateStepTest
        {
            get
            {
                WorkspaceViewModel wvm;

                if ((wvm = GetActiveSelectedObject()) == null)
                {
                    return false;
                }

                return wvm is UserViewModel;
            }
        }

        public void GenerateStepTestPdf(StepTestViewModel stepTest)
        {
            if (stepTest == null)
            {
                stepTest = GetActiveSelectedObject() as StepTestViewModel;
            }

            ContentWindow modalWindow = null;
            var selectedList = new List<StepTestViewModel>();
            var viewModel = new UserStepTestListViewModel(stepTest, this, (p, dr) =>
            {
                if (p != null)
                {
                    selectedList = p.ToList();
                }
                modalWindow.DialogResult = dr;
                modalWindow.Close();
            });

            var view = new UserStepTestListView { DataContext = viewModel };
            modalWindow = new ContentWindow
            {
                Content = view
            };
            modalWindow.ShowDialog();
            if (modalWindow.DialogResult.HasValue && modalWindow.DialogResult.Value)
            {
                var generator = new StepTestReport(stepTest.Source, selectedList.Select(s => s.Source));
                var pdfDocument = generator.CreateReport();
                var filename = $"{stepTest.Source.ParentUser.FirstName} {stepTest.Source.ParentUser.LastName} ({stepTest.Source.Id}).pdf";
                generator.PdfRender(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename), pdfDocument);

                MessageBox.Show($"PDF {filename} is generated", "PDF Generation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Measurement Helpers

        public void CreateNewMeasurement(object stepTestObject)
        {
            // First we must have a step test view model object
            StepTestViewModel stepTest = null;

            // Check parameter
            if (stepTestObject is StepTestViewModel)
            {
                stepTest = stepTestObject as StepTestViewModel;
            }

            // Try to get it from current workspace
            else
            {
                var wvm = GetActiveWorkspace();
                if (wvm is AllStepTestsViewModel avm)
                {
                    stepTest = avm.AllStepTests.FirstOrDefault(item => item.IsSelected);
                }

                else if (wvm is StepTestViewModel stvm)
                {
                    stepTest = stvm;
                }

                else if (wvm is AllMeasurementsViewModel amvm)
                {
                    stepTest = amvm.ParentViewModel;
                }
            }

            var newSequence = stepTest.Source.Measurements.Count == 0 ? 1 : stepTest.Source.Measurements.Max(m => m.Sequence) + 1;
            var newLoad = stepTest.Source.Measurements.Count == 0 ? stepTest.Source.LoadPreset : stepTest.Source.Measurements.Last().Load + stepTest.Source.Increase;

            var newMeasurement = Measurement.Create(newSequence, stepTest.StepTestId, 0, 0, newLoad);
            newMeasurement.InCalculation = true;
            newMeasurement.ParentStepTest = stepTest.Source;
            newMeasurement.AcceptChanges();
            stepTest.Source.Measurements.Add(newMeasurement);
            var workspace = new MeasurementViewModel(newMeasurement, this);
            ShowWorkspace(workspace);
        }

        public void ShowMeasurement(MeasurementViewModel measurement)
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is MeasurementViewModel && (vm as MeasurementViewModel).MeasurementId.Equals(measurement.MeasurementId)) is MeasurementViewModel workspace))
            {
                workspace = measurement;
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        public void ShowAllMeasurements(StepTestViewModel stepTest)
        {
            if (stepTest == null)
            {
                stepTest = GetActiveSelectedObject() as StepTestViewModel;
            }

            var workspace = new AllMeasurementsViewModel(stepTest, this);
            ShowWorkspace(workspace);
        }

        public bool CanShowAllMeasurements
        {
            get
            {
                WorkspaceViewModel wvm;

                if ((wvm = GetActiveSelectedObject()) == null)
                {
                    return false;
                }

                return wvm is StepTestViewModel;
            }
        }

        public bool CanCreateMeasurement
        {
            get
            {
                WorkspaceViewModel wvm;

                if ((wvm = GetActiveSelectedObject()) == null)
                {
                    if ((wvm = GetActiveWorkspace()) == null)
                    {
                        return false;
                    }

                    return wvm is AllMeasurementsViewModel;
                }

                return wvm is StepTestViewModel;
            }
        }

        public bool CanGenerateStepTestPdf
        {
            get
            {
                WorkspaceViewModel wvm;
                if ((wvm = GetActiveSelectedObject()) == null)
                {
                    return false;
                }

                return wvm is StepTestViewModel;
            }
        }

        #endregion

        #region General

        public string CurrentDatabaseFilename { get; private set; }

        public bool IsDatabaseOpen { get; set; }

        public void GenerateCalculation(IEnumerable<StepTestViewModel> viewModels)
        {
            //var calculation = new FlbcCalculation(viewModel.Source.Measurements, 4.0);
            //var zonesCalc = new LactateBasedZones(calculation, new[] { 0.8, 1.5, 2.5, 4.0, 6.0, 10.0 });
            //var zones = zonesCalc.Zones.ToList();
            //var message = new StringBuilder($"LT={calculation.LactateThreshold}, HR={calculation.HeartRateThreshold}\r\n");
            //foreach (var zone in zones)
            //{
            //    message.AppendLine(zone.ToString());
            //}
            //MessageBox.Show(message.ToString());

            var workspace = new StepTestsPlotViewModel(viewModels);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        #endregion

        /// <summary>
        /// Sets the active workspace.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(Workspaces.Contains(workspace));

            var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
        }

        /// <summary>
        /// Gets the active workspace.
        /// </summary>
        /// <returns></returns>
        private WorkspaceViewModel GetActiveWorkspace()
        {
            var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
            {
                return collectionView.CurrentItem as WorkspaceViewModel;
            }

            return null;
        }

        /// <summary>
        /// Gets the active selected object.
        /// </summary>
        /// <returns></returns>
        private WorkspaceViewModel GetActiveSelectedObject()
        {
            WorkspaceViewModel currentViewModel;
            return (currentViewModel = GetActiveWorkspace()) != null ? currentViewModel.SelectedObject : null;
        }

        private async void SaveMru(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path is null or empty!", nameof(path));
            }

            try
            {
                IsProcessingMru = true;
                var t = await MRUEntrySerializer.SaveAsync(path, MRUFileList);
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace, exp.Message);
            }

            finally
            {
                IsProcessingMru = false;
            }
        }

        public void SaveMru()
        {
            SaveMru(_mruPersistPath);
        }

        private async void LoadMru(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path is null or empty!", nameof(path));
            }

            try
            {
                IsProcessingMru = true;
                SetMruList(await MRUEntrySerializer.LoadAsync(path));
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace, exp.Message);
            }

            finally
            {
                IsProcessingMru = false;
            }
        }

        private void SetMruList(IMRUListViewModel mruFilelist)
        {
            if (mruFilelist == null)
            {
                return;
            }

            MRUFileList = mruFilelist;
            OnPropertyChanged(nameof(MRUFileList));
        }

        #endregion // Private Helpers

        #region Public Methods

        /// <summary>
        /// Gets the selected object.
        /// </summary>
        public override WorkspaceViewModel SelectedObject
        {
            get { return this; }
        }

        /// <summary>
        /// Shows the workspace.
        /// </summary>
        /// <param name="view">The view.</param>
        public void ShowWorkspace(WorkspaceViewModel view)
        {
            if (!Workspaces.Contains(view))
            {
                Workspaces.Add(view);
            }

            SetActiveWorkspace(view);
        }

        #endregion
    }
}
