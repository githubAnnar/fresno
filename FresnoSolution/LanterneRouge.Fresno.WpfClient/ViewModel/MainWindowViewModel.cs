using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using log4net;
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
    public class MainWindowViewModel : ViewModelBase /* : WorkspaceViewModel, IWorkspaceCommands */
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindowViewModel));

        private ObservableCollection<CommandViewModel> _commands;
        private ObservableCollection<WorkspaceViewModel> _workspaces;
        private ICommand _openFileCommand;
        private ICommand _newFileCommand;
        private ICommand _navigateUriCommand;
        private readonly string _mruPersistPath;
        private bool _isProcessingMru;
        private ICommand _closeCommand;
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
            Logger.Debug("Checking MRU file");
            var mruDriectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppDirecory}";
            if (!Directory.Exists(mruDriectory))
            {
                Directory.CreateDirectory(mruDriectory);
                Logger.Debug($"Created MRU file directory: {mruDriectory}");
            }

            else
            {
                Logger.Debug($"MRU directory '{mruDriectory}' already exists");
            }

            _mruPersistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{AppDirecory}\\{MruFilename}";
            if (!File.Exists(_mruPersistPath))
            {
                MRUFileList = MRU_Service.Create_List();
                Logger.Debug("Created new MRU List because none is found");
            }

            else
            {
                LoadMru(_mruPersistPath);
                Logger.Debug($"Loaded MRU File '{_mruPersistPath}'");
            }
        }

        #endregion // Constructor

        #region Properties
        private WorkspaceViewModel _selectedItem;
        public WorkspaceViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                Commands = _selectedItem?.SubCommands;
            }
        }

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
        public ObservableCollection<CommandViewModel> Commands
        {
            get => _commands ?? (_commands = new ObservableCollection<CommandViewModel>(GetInitialCommands()));
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Creates the commands.
        /// </summary>
        /// <returns></returns>
        private List<CommandViewModel> GetInitialCommands() => new List<CommandViewModel>
            {
                new CommandViewModel("All Users", new RelayCommand(param => ShowAllUsers(), param=>IsDatabaseOpen))
            };

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(param => OnRequestClose()));

        #endregion // CloseCommand

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
                    Logger.Debug($"Opened database '{CurrentDatabaseFilename}'");
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
                    Logger.Debug($"Created new database '{CurrentDatabaseFilename}'");
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
                Logger.Debug($"Opened database from MRU '{path}'");
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

            var newWS = GetActiveWorkspace();
            if (newWS != null)
            {
                newWS.Show();
            }

            else
            {
                Commands = null;
            }

        }

        #endregion // Workspaces

        #region Private Helpers

        #region User Helpers

        private void ShowAllUsers()
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is AllUsersViewModel) is AllUsersViewModel workspace))
            {
                workspace = new AllUsersViewModel(ShowWorkspace);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
            Logger.Debug("\"All users\" workspace is shown");

            SetActiveCommands(workspace);
        }

        #endregion

        #region General

        public string CurrentDatabaseFilename { get; private set; }

        public bool IsDatabaseOpen { get; set; }

        #endregion

        /// <summary>
        /// Sets the active workspace.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            if (!Workspaces.Contains(workspace))
            {
                Logger.Warn($"Workspace \"{workspace}\" is not found in collection!");
            }

            var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
                Logger.Debug($"Changed workspace to \"{workspace}\"");
            }
        }

        public void SetActiveCommands(WorkspaceViewModel workspace)
        {
            Commands = workspace.SubCommands;
            Logger.Debug($"Changed commands for workspace \"{workspace}\"");

            if (!ContainsWorkspace(typeof(AllUsersViewModel)))
            {
                Commands.Insert(0, new CommandViewModel("All Users", new RelayCommand(param => ShowAllUsers(), param => IsDatabaseOpen)));
            }
        }

        private bool ContainsWorkspace(Type workspaceType) => Workspaces.Any(w => w.GetType().Equals(workspaceType));

        /// <summary>
        /// Gets the active workspace.
        /// </summary>
        /// <returns></returns>
        private WorkspaceViewModel GetActiveWorkspace()
        {
            var collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            return collectionView != null ? collectionView.CurrentItem as WorkspaceViewModel : null;
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
                Logger.Error("save MRU path is null or empty!");
                throw new ArgumentException("Path is null or empty!", nameof(path));
            }

            try
            {
                IsProcessingMru = true;
                var t = await MRUEntrySerializer.SaveAsync(path, MRUFileList);
                Logger.Info($"MRU is saved to '{path}'");
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace, exp.Message);
                Logger.Error($"Error saving MRU to '{path}'", exp);
            }

            finally
            {
                IsProcessingMru = false;
            }
        }

        public void SaveMru() => SaveMru(_mruPersistPath);

        private async void LoadMru(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Logger.Error($"Load MRU path is null or empty!");
                throw new ArgumentException("Path is null or empty!", nameof(path));
            }

            try
            {
                IsProcessingMru = true;
                SetMruList(await MRUEntrySerializer.LoadAsync(path));
                Logger.Info($"MRU is loaded from '{path}'");
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.StackTrace, exp.Message);
                Logger.Error($"Error loading MRU from '{path}'", exp);
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
                Logger.Debug("mruFileList is null");
                return;
            }

            MRUFileList = mruFilelist;
            Logger.Info("mruFileList is set!");
            OnPropertyChanged(nameof(MRUFileList));
        }

        #endregion // Private Helpers

        #region Public Methods

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
            SetActiveCommands(view);
        }

        #endregion

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Called when [request close].
        /// </summary>
        private void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);

        #endregion // RequestClose [event]
    }
}
