using Autofac;
using LanterneRouge.Fresno.Services;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Wpf.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public abstract class WorkspaceViewModel(WorkspaceViewModel parent, BitmapImage icon) : ViewModelBase, IWorkspaceCommands
    {
        #region Fields

        private ICommand _closeCommand;
        private readonly ObservableCollection<CommandViewModel> _subCommands;
        private ObservableCollection<CommandViewModel> _contextMenuItemCommands;
        private IDataService _manager;

        #endregion // Fields

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(param => OnRequestClose());

        #endregion // CloseCommand

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

        #region Abstract Methods

        /// <summary>
        /// Gets the selected object.
        /// </summary>
        public abstract WorkspaceViewModel SelectedObject { get; }

        /// <summary>
        /// Gets the data object.
        /// </summary>
        //public abstract IDbEntity<TEntity> DataObject { get; }

        #endregion

        #region Public Properties

        public IDataService DataManager
        {
            get
            {
                if (_manager == null)
                {
                    var scope = ServiceLocator.Instance.BeginLifetimeScope();
                    _manager = scope.Resolve<IDataService>();
                }

                return _manager;
            }
        }

        public BitmapImage ItemIcon { get; protected set; } = icon ?? new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-report-card-100.png"));

        public WorkspaceViewModel Parent { get; } = parent;

        internal MainWindowViewModel RootViewModel { get; set; }

        #endregion

        #region IWorkspaceCommands2 Methods

        public ObservableCollection<CommandViewModel> SubCommands { get; set; } = [];

        public ObservableCollection<CommandViewModel> ContextMenuItemCommands
        {
            get => _contextMenuItemCommands;
            set => _contextMenuItemCommands = value;
        }

        public void Show() => RootViewModel.ShowWorkspace(this);

        public abstract void CreateChild();

        #endregion
    }
}
