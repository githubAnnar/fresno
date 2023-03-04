using System.Windows.Controls;

namespace LanterneRouge.Wpf.MVVM
{
    public class CustomTabItem
    {
        public string Header { get; set; }

        public UserControl Content { get; set; }

        public bool IsSelected { get; set; }
    }
}
