using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Services;
using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using System;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _closeCommand;

        #endregion // Fields

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
        private void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

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
        public DataService DataManager { get => _manager ?? (_manager = ServiceLocator.Instance.GetService(typeof(IDataService)) as DataService); }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        //public WorkspaceViewModel Parent { get; set; }

        #endregion
    }
}
