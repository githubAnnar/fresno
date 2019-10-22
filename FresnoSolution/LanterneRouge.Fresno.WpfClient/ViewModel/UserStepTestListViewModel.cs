using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class UserStepTestListViewModel : WorkspaceViewModel
    {
        #region Constructors

        public UserStepTestListViewModel(StepTestViewModel baseStepTestViewModel, IWorkspaceCommands mainWorkspaceViewModel, Action<IEnumerable<StepTestViewModel>> closeAction)
        {
            WsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
            BaseStepTestViewModel = baseStepTestViewModel ?? throw new ArgumentNullException(nameof(baseStepTestViewModel));
            CloseAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
        }

        #endregion

        #region Properties

        private IWorkspaceCommands WsCommands { get; }

        private Action<IEnumerable<StepTestViewModel>> CloseAction { get; }

        public StepTestViewModel BaseStepTestViewModel { get; }

        public List<StepTestViewModel> AdditionalStepTestCandidates => (from st in DataManager.GetAllStepTestsByUser(BaseStepTestViewModel.Source.ParentUser).Where(st => st.Id != BaseStepTestViewModel.Source.Id) select new StepTestViewModel(st, WsCommands)).ToList();

        public List<StepTestViewModel> SelectedStepTests { get; set; }
        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Methods

        private void Ok(object p)
        {
            System.Collections.IList items = (System.Collections.IList)p; 
            var selection = items?.Cast<StepTestViewModel>();
            CloseAction(selection);
        }

        #endregion

        #region Commands

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new RelayCommand(p => Ok(p)));

        #endregion
    }
}
