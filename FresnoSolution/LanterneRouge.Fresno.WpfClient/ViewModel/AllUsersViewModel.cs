using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllUsersViewModel : WorkspaceViewModel
    {
        #region Fields

        private static readonly string _name = typeof(AllUsersViewModel).Name;
        private readonly IWorkspaceCommands _wsCommands;

        #endregion

        #region Constructors

        public AllUsersViewModel(IWorkspaceCommands mainWorkspaceViewModel)
        {
            DisplayName = "All Users"; /*KayakStrings.Race_All_Races;*/
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException("mainWorkspaceViewModel");
            CreateAllUsers();
        }

        private void CreateAllUsers()
        {
            var all = (from user in DataManager.GetAllUsers(true) select new UserViewModel(user, _wsCommands)).ToList();

            foreach (var rvm in all)
            {
                rvm.PropertyChanged += OnUserViewModelPropertyChanged;
            }

            AllUsers = new ObservableCollection<UserViewModel>(all);
            AllUsers.CollectionChanged += OnCollectionChanged;
        }

        #endregion

        #region Public Interface

        public ObservableCollection<UserViewModel> AllUsers { get; private set; }

        public UserViewModel Selected => SelectedObject as UserViewModel;

        public static string GetIdentifierName()
        {
            return _name;
        }

        #endregion

        #region Base Class Overrides

        protected override void OnDispose()
        {
            foreach (UserViewModel userVM in AllUsers)
            {
                userVM.Dispose();
            }

            AllUsers.Clear();
            AllUsers.CollectionChanged -= OnCollectionChanged;
        }

        public override WorkspaceViewModel SelectedObject => AllUsers.FirstOrDefault(item => item.IsSelected);

        #endregion

        #region Event Handling Methods

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && !e.NewItems.Count.Equals(0))
            {
                foreach (UserViewModel userVM in e.NewItems)
                {
                    userVM.PropertyChanged += OnUserViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (UserViewModel userVM in e.OldItems)
                {
                    userVM.PropertyChanged -= OnUserViewModelPropertyChanged;
                }
            }
        }

        private void OnUserViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string isSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            if (sender is UserViewModel raceViewModel) raceViewModel.VerifyPropertyName(isSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            //if (e.PropertyName.Equals(isSelected))
            //{
            //    this.OnPropertyChanged("TotalSelectedSales");
            //}
        }

        #endregion
    }
}
