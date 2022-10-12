using LanterneRouge.Fresno.WpfClient.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class FresnoToolWindowViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<UserControl> _tabItems;

        #endregion
        public ObservableCollection<UserControl> TabItems
        {
            get => _tabItems;
            set
            {
                _tabItems = value;
                OnPropertyChanged();
            }
        }
    }
}
