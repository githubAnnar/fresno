using LanterneRouge.Fresno.WpfClient.ViewModel;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.MVVM
{
    public interface IWorkspaceCommands2
    {
        void Show();

        void Show(WorkspaceViewModel viewModel);

        bool CanShow(WorkspaceViewModel viewModel);

        void Create(WorkspaceViewModel viewModel);

        bool CanCreate(WorkspaceViewModel viewModel);

        ObservableCollection<CommandViewModel> SubCommands { get; set; }
    }
}
