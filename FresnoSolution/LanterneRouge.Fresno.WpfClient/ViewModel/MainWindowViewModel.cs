﻿using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class MainWindowViewModel : WorkspaceViewModel, IWorkspaceCommands
    {
        #region Fields

        private ReadOnlyCollection<CommandViewModel> _commands;
        private ObservableCollection<WorkspaceViewModel> _workspaces;
        private ICommand _openFileCommand = null;
        private ICommand _newFileCommand = null;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            DisplayName = $"Step Test Viewer";
        }

        #endregion // Constructor

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
                new CommandViewModel($"Create New Step Test", new RelayCommand(param => CreateNewStepTest(param), param =>CanCreateStepTest)),
                new CommandViewModel("All Measurements", new RelayCommand(param=> ShowAllMeasurements(param as StepTestViewModel), param=>CanShowAllMeasurements)),
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
                }
            }
        }

        #endregion

        #region NewFileCommand

        public ICommand NewFileCommand => _newFileCommand ?? (_newFileCommand = new RelayCommand(param => NewFile()));

        private void NewFile()
        {
            throw new NotImplementedException();
        }

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
            var workspace = new UserViewModel(newUser, this);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
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
            }

            var newMeasurement = Measurement.Create(1, stepTest.StepTestId, 0, 0, 0);
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
                    return false;
                }

                return wvm is StepTestViewModel;
            }
        }

        #endregion

        #region General

        public string CurrentDatabaseFilename { get; private set; }

        public bool IsDatabaseOpen { get; set; }

        public void GenerateCalculation(StepTestViewModel viewModel)
        {

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