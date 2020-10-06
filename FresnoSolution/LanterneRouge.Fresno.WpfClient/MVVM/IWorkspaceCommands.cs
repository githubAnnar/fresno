using LanterneRouge.Fresno.WpfClient.ViewModel;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.MVVM
{
    public interface IWorkspaceCommands
    {
        void Show();

        void CreateChild();

        ObservableCollection<CommandViewModel> SubCommands { get; set; }

        ObservableCollection<CommandViewModel> ContextMenuItemCommands { get; set; }
    }
}
