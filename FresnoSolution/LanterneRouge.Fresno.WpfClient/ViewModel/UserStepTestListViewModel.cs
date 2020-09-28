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

        public UserStepTestListViewModel(UserViewModel user, StepTestViewModel baseStepTestViewModel, Action<IEnumerable<StepTestViewModel>, bool> closeAction, Action<WorkspaceViewModel> showWorkspace) : base(user, showWorkspace, null)
        {
            BaseStepTestViewModel = baseStepTestViewModel ?? throw new ArgumentNullException(nameof(baseStepTestViewModel));
            CloseAction = closeAction ?? throw new ArgumentNullException(nameof(closeAction));
        }

        #endregion

        #region Properties

        private UserViewModel UserParent => Parent as UserViewModel;

        private Action<IEnumerable<StepTestViewModel>, bool> CloseAction { get; }

        public StepTestViewModel BaseStepTestViewModel { get; }

        public List<StepTestViewModel> AdditionalStepTestCandidates => (from st in DataManager.GetAllStepTestsByUser(BaseStepTestViewModel.Source.ParentUser).Where(st => st.Id != BaseStepTestViewModel.Source.Id) select new StepTestViewModel(st, UserParent, ShowWorkspace)).ToList();

        public List<StepTestViewModel> SelectedStepTests { get; set; }
        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Methods

        private void Ok(object p)
        {
            System.Collections.IList items = (System.Collections.IList)p;
            var selection = items?.Cast<StepTestViewModel>();
            CloseAction(selection, true);
        }

        private void Cancel() => CloseAction(null, false);

        #endregion

        #region Commands

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new RelayCommand(p => Ok(p)));

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(p => Cancel()));

        #endregion
    }
}
