using System;
using System.Collections.ObjectModel;
using LanterneRouge.Fresno.netcore.AvaloniaClient.ViewModels;

namespace LanterneRouge.Fresno.netcore.AvaloniaClient.MVVM
{
    public interface IWorkspaceCommands
    {
        void Show();

        void CreateChild();

        ObservableCollection<CommandViewModel> SubCommands { get; set; }

        ObservableCollection<CommandViewModel> ContextMenuItemCommands { get; set; }
    }
}
