using LanterneRouge.Fresno.WpfClient.MVVM;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class FresnoToolWindowViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<CustomTabItem> _tabItems;

        #endregion
        public ObservableCollection<CustomTabItem> TabItems
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
