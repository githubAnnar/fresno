using LanterneRouge.Wpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class UserStepTestListViewModel(UserViewModel user, StepTestViewModel baseStepTestViewModel, Action<IEnumerable<StepTestViewModel>, bool> closeAction) : WorkspaceViewModel(user, null)
    {
        #region Properties

        private UserViewModel UserParent => Parent as UserViewModel;

        private Action<IEnumerable<StepTestViewModel>, bool> CloseAction { get; } = closeAction ?? throw new ArgumentNullException(nameof(closeAction));

        public StepTestViewModel BaseStepTestViewModel { get; } = baseStepTestViewModel ?? throw new ArgumentNullException(nameof(baseStepTestViewModel));

        public List<StepTestViewModel> AdditionalStepTestCandidates => (from st in DataManager.GetAllStepTestsByUser(DataManager.GetUserByStepTest(BaseStepTestViewModel.Source).Result).Result.Where(st => st.Id != BaseStepTestViewModel.Source.Id) select new StepTestViewModel(st, UserParent)).ToList();

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

        public override void CreateChild() => throw new NotImplementedException();

        #endregion

        #region Commands

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ??= new RelayCommand(p => Ok(p));

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(p => Cancel());

        #endregion
    }
}
