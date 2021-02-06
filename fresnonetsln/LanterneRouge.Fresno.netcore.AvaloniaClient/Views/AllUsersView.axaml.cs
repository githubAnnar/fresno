using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LanterneRouge.Fresno.netcore.AvaloniaClient.Views
{
    public class AllUsersView : UserControl
    {
        public AllUsersView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}