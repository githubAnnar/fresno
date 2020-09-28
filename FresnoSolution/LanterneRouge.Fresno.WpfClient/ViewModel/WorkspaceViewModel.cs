using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public abstract class WorkspaceViewModel : ViewModelBase, IWorkspaceCommands2
    {
        #region Fields

        private ICommand _closeCommand;

        #endregion // Fields

        #region Constructors

        public WorkspaceViewModel(WorkspaceViewModel parent, Action<WorkspaceViewModel> showWorkspace, BitmapImage icon)
        {
            Parent = parent;
            ShowWorkspace = showWorkspace ?? throw new ArgumentNullException(nameof(showWorkspace));
            ItemIcon = icon ?? new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-report-card-100.png"));
        }

        #endregion

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(param => OnRequestClose()));

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

        private DataService _manager;
        public DataService DataManager => _manager ?? (_manager = ServiceLocator.Instance.GetService(typeof(IDataService)) as DataService);

        public BitmapImage ItemIcon { get; protected set; }

        public WorkspaceViewModel Parent { get; }

        public Action<WorkspaceViewModel> ShowWorkspace { get; }

        #endregion

        #region IWorkspaceCommands2 Methods

        public abstract ObservableCollection<CommandViewModel> SubCommands { get; set; }

        public void Show()
        {
            ShowWorkspace(this);
        }

        public void Show(WorkspaceViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public bool CanShow(WorkspaceViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public abstract void Create(WorkspaceViewModel viewModel);

        public bool CanCreate(WorkspaceViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
