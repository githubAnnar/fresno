using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class UserStepTestListViewModel : WorkspaceViewModel
    {
        #region Constructors

        public UserStepTestListViewModel(StepTestViewModel baseStepTestViewModel, IWorkspaceCommands mainWorkspaceViewModel)
        {
            WsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
            BaseStepTestViewModel = baseStepTestViewModel ?? throw new ArgumentNullException(nameof(baseStepTestViewModel));
        }

        #endregion

        #region Properties

        private IWorkspaceCommands WsCommands { get; }

        public StepTestViewModel BaseStepTestViewModel { get; }

        public List<StepTestViewModel> AdditionalStepTestCandidates => (from st in DataManager.GetAllStepTestsByUser(BaseStepTestViewModel.Source.ParentUser).Where(st => st.Id != BaseStepTestViewModel.Source.Id) select new StepTestViewModel(st, WsCommands)).ToList();

        public List<StepTestViewModel> SelectedStepTests { get; set; }

        #endregion

        public override WorkspaceViewModel SelectedObject => this;
    }
}
